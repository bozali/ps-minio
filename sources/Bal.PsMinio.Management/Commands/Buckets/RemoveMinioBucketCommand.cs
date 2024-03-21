using System.Management.Automation;

using Minio.DataModel.Args;

namespace Bal.PsMinio.Management.Commands.Buckets;

[Cmdlet(VerbsCommon.Remove, "MinioBucket")]
public class RemoveMinioBucketCommand : BucketOperation
{
    protected override void ProcessRecord()
    {
        var args = new RemoveBucketArgs().WithBucket(this.GetBucketName());
        this.Client.Context.RemoveBucketAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
    }
}