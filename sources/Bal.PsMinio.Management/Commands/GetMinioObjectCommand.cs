using System.Management.Automation;

using Bal.PsMinio.Management.Domain;

using Minio.DataModel;
using Minio.DataModel.Args;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Bal.PsMinio.Management.Commands;

[Cmdlet(VerbsCommon.Get, "MinioObject")]
public class GetMinioObjectCommand : PSCmdlet
{
    [Parameter]
    public MinioClientObject Client { get; set; }

    [Parameter]
    public Bucket Bucket { get; set; }

    [Parameter]
    public string BucketName { get; set; }

    [Parameter]
    public string[] Name { get; set; }

    protected override void ProcessRecord()
    {
        var args = new ListObjectsArgs();

        if (!string.IsNullOrEmpty(this.BucketName))
        {
            args = args.WithBucket(this.BucketName);
        }
        else
        {
            args = args.WithBucket(this.Bucket.Name);
        }

        var items = new List<Item>();

        using var subscription = this.Client.Minio.ListObjectsAsync(args).Subscribe(item => items.Add(item));

        this.WriteObject(items);
    }
}