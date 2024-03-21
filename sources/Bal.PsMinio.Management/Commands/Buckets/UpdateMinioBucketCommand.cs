using System.Collections;
using System.Collections.ObjectModel;
using System.Management.Automation;
using Minio.DataModel.Args;
using Minio.DataModel.ILM;
using Minio.DataModel.Tags;
using Minio.Exceptions;

namespace Bal.PsMinio.Management.Commands.Buckets;

[Cmdlet(VerbsData.Update, "MinioBucket")]
public class UpdateMinioBucketCommand : BucketOperation
{
    [Parameter]
    public Hashtable? Tags { get; set; }

    [Parameter]
    public SwitchParameter EnableVersioning { get; set; }

    protected override void ProcessRecord()
    {
        string bucketName = this.GetBucketName();

        if (this.EnableVersioning)
        {
            this.WriteVerbose("Enable versioning");

            var versioningArgs = new SetVersioningArgs()
                .WithBucket(bucketName)
                .WithVersioningEnabled();

            this.Client.Context.SetVersioningAsync(versioningArgs).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        if (this.Tags != null && this.Tags.Keys.Count != 0)
        {
            this.WriteVerbose("Setting tags");

            var tagArgs = new SetBucketTagsArgs()
                .WithBucket(bucketName)
                .WithTagging(new Tagging(this.Tags.Cast<DictionaryEntry>().ToDictionary(x => (string)x.Key!, y => (string)y.Value!), false));

            this.Client.Context.SetBucketTagsAsync(tagArgs).ConfigureAwait(true).GetAwaiter().GetResult();
        }

        var bucketsResponse = this.Client.Context.ListBucketsAsync().ConfigureAwait(true).GetAwaiter().GetResult();
        var bucket = bucketsResponse.Buckets.FirstOrDefault(x => x.Name == bucketName);

        if (bucket == null)
        {
            throw new MinioException($"Could not create bucket");
        }

        this.WriteObject(bucket);
    }
}