using System.Management.Automation;

using Minio.DataModel;
using Minio.DataModel.Args;

namespace Bal.PsMinio.Management.Commands;

public class BucketOperation : ClientOperation
{
    [Parameter(ParameterSetName = "BucketName")]
    public string? BucketName { get; set; }

    [Parameter(ParameterSetName = "Bucket")]
    public Bucket? Bucket { get; set; }

    protected string GetBucketName()
    {
        string? bucketName = string.Equals(this.ParameterSetName, "Bucket", StringComparison.OrdinalIgnoreCase)
            ? this.Bucket?.Name
            : this.BucketName;

        if (string.IsNullOrEmpty(bucketName))
        {
            throw new Exception("Bucket name is missing");
        }

        if (this.Client.Minio
            .BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName))
            .ConfigureAwait(false)
            .GetAwaiter().GetResult())
        {
            throw new Exception($"Bucket {bucketName} does not exist");
        }

        return bucketName;
    }
}