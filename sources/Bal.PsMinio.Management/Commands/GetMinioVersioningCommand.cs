using System.Management.Automation;

using Minio.DataModel.Args;

namespace Bal.PsMinio.Management.Commands;

[Cmdlet(VerbsCommon.Get, "MinioVersioning")]
public class GetMinioVersioningCommand : BucketOperation
{
    protected override void ProcessRecord()
    {
        var args = new GetVersioningArgs().WithBucket(this.GetBucketName());
        var result = this.Client.Minio.GetVersioningAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        this.WriteObject(result.Status);
    }
}