using System.Collections;
using System.Management.Automation;

using Bal.PsMinio.Management.Domain;
using Bal.PsMinio.Management.Commands.Enums;

using Minio.DataModel.Args;
using Minio.DataModel.ObjectLock;
using Minio.DataModel.Tags;
using Minio.Exceptions;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Bal.PsMinio.Management.Commands.Buckets;

[Cmdlet(VerbsCommon.New, "MinioBucket")]
public class NewMinioBucketCommand : PSCmdlet
{
    [ValidateNotNull]
    [Parameter]
    public MinioClientObject Client { get; set; }

    [ValidateNotNull]
    [Parameter]
    public string Name { get; set; }

    [Parameter]
    public Hashtable? Tags { get; set; }

    [Parameter]
    public SwitchParameter EnableVersioning { get; set; }

    [Parameter]
    public SwitchParameter EnableObjectLocking { get; set; }

    [Parameter]
    public RetentionMode? RetentionMode { get; set; }

    [Parameter]
    public int Days { get; set; } = 100;

    protected override void ProcessRecord()
    {
        this.WriteVerbose($"Checking if bucket {this.Name} exists");

        if (this.Client.Context
            .BucketExistsAsync(new BucketExistsArgs().WithBucket(this.Name))
            .GetAwaiter()
            .GetResult())
        {
            throw new MinioException($"The bucket {this.Name} already exists");
        }

        this.WriteVerbose($"Preparing bucket arguments");

        var bucketArgs = new MakeBucketArgs()
            .WithBucket(this.Name);

        bool enableRetention = this.RetentionMode != null;
        bool enableObjectLocking = this.EnableObjectLocking || enableRetention;
        bool enableVersioning = this.EnableVersioning || enableObjectLocking;

        if (enableObjectLocking)
        {
            bucketArgs = bucketArgs.WithObjectLock();
        }

        this.WriteVerbose($"Creating bucket {this.Name}");

        this.Client.Context.MakeBucketAsync(bucketArgs).GetAwaiter().GetResult();

        if (enableVersioning)
        {
            this.WriteVerbose("Enable versioning");

            var versioningArgs = new SetVersioningArgs()
                .WithBucket(this.Name)
                .WithVersioningEnabled();

            this.Client.Context.SetVersioningAsync(versioningArgs).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        if (enableRetention)
        {
            this.WriteVerbose("Enable object locking");

            var configuration = new ObjectLockConfiguration(
                this.RetentionMode == Enums.RetentionMode.Compliance
                    ? ObjectRetentionMode.COMPLIANCE
                    : ObjectRetentionMode.GOVERNANCE, this.Days);

            var lockArgs = new SetObjectLockConfigurationArgs()
                .WithBucket(this.Name)
                .WithLockConfiguration(configuration);

            this.Client.Context.SetObjectLockConfigurationAsync(lockArgs).GetAwaiter().GetResult();
        }

        var bucketsResponse = this.Client.Context.ListBucketsAsync().ConfigureAwait(true).GetAwaiter().GetResult();
        var bucket = bucketsResponse.Buckets.FirstOrDefault(x => x.Name == this.Name);

        if (bucket == null)
        {
            throw new MinioException($"Could not create bucket");
        }

        if (this.Tags != null && this.Tags.Keys.Count != 0)
        {
            this.WriteVerbose("Setting tags");

            var tagArgs = new SetBucketTagsArgs()
                .WithBucket(this.Name)
                .WithTagging(new Tagging(this.Tags.Cast<DictionaryEntry>().ToDictionary(x => (string)x.Key!, y => (string)y.Value!), false));

            this.Client.Context.SetBucketTagsAsync(tagArgs).ConfigureAwait(true).GetAwaiter().GetResult();
        }

        this.WriteObject(bucket);
    }
}