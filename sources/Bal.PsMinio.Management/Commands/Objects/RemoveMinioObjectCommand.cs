using System.Management.Automation;
using Minio.DataModel.Args;

namespace Bal.PsMinio.Management.Commands.Objects;

[Cmdlet(VerbsCommon.Remove, "MinioObject")]
public class RemoveMinioObjectCommand : BucketOperation
{
    [Parameter(Mandatory = true)]
    public string[] Objects { get; set; }

    protected override void ProcessRecord()
    {
        string bucketName = this.GetBucketName();
    }
}