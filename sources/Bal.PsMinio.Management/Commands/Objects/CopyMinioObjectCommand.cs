using System.Management.Automation;

using Minio.DataModel;
using Minio.DataModel.Args;

namespace Bal.PsMinio.Management.Commands.Objects;

[Cmdlet(VerbsCommon.Copy, "MinioObject")]
public class CopyMinioObjectCommand : ClientOperation
{
    [Parameter]
    public string? SourceBucketName { get; set; }

    [Parameter]
    public Bucket? SourceBucket { get; set; }

    [Parameter]
    public string? DestinationBucketName { get; set; }

    [Parameter]
    public Bucket? DestinationBucket { get; set; }

    [Parameter]
    public string? SourceObject { get; set; }

    [Parameter]
    public string? DestinationObject { get; set; }

    protected override void ProcessRecord()
    {
        if (string.IsNullOrEmpty(this.SourceObject))
        {
            throw new Exception($"No source object was provided");
        }

        if (string.IsNullOrEmpty(this.DestinationObject))
        {
            throw new Exception($"No destination object was provided");
        }

        this.WriteVerbose($"Preparing source copy arguments");

        var sourceArgs = new CopySourceObjectArgs()
            .WithBucket(this.GetSourceBucketName())
            .WithObject(this.SourceObject);

        this.WriteVerbose($"Preparing copy arguments");

        var copyArgs = new CopyObjectArgs()
            .WithBucket(this.GetDestinationBucketName())
            .WithObject(this.DestinationObject)
            .WithCopyObjectSource(sourceArgs);

        this.WriteVerbose($"Copying object to destination");

        this.Client.Context.CopyObjectAsync(copyArgs).ConfigureAwait(false).GetAwaiter().GetResult();

        this.WriteVerbose($"Copied object {this.SourceObject} from bucket {this.GetSourceBucketName()} to object {this.DestinationObject}");
    }

    private string GetSourceBucketName()
    {
        if (!string.IsNullOrEmpty(this.SourceBucketName))
        {
            return this.SourceBucketName;
        }

        if (this.SourceBucket != null)
        {
            return this.SourceBucket.Name;
        }

        throw new Exception("No source bucket name was provided");
    }

    private string GetDestinationBucketName()
    {
        if (!string.IsNullOrEmpty(this.DestinationBucketName))
        {
            return this.DestinationBucketName;
        }

        if (this.DestinationBucket != null)
        {
            return this.DestinationBucket.Name;
        }

        string destination = this.GetSourceBucketName();

        if (!string.IsNullOrEmpty(destination))
        {
            return destination;
        }

        throw new Exception("No destination bucket name was provided");
    }
}