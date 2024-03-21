using System.Management.Automation;

using Minio.DataModel;
using Minio.DataModel.Args;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Bal.PsMinio.Management.Commands.Objects;

[Cmdlet(VerbsCommon.Get, "MinioObject")]
public class GetMinioObjectCommand : BucketOperation
{
    [Parameter]
    public string[] Name { get; set; }

    protected override void ProcessRecord()
    {
        string bucketName = this.GetBucketName();

        var args = new ListObjectsArgs().WithBucket(bucketName);
        var items = new List<Item>();

        if (this.Name?.Any() == true)
        {
            using var subscription = this.Client.Context.ListObjectsAsync(args).Subscribe(item =>
            {
                foreach (string name in this.Name)
                {
                    var wildcard = new WildcardPattern(name, WildcardOptions.IgnoreCase | WildcardOptions.Compiled);

                    if (wildcard.IsMatch(item.Key))
                    {
                        items.Add(item);
                    }
                }
            });

            this.WriteObject(items);
            return;
        }

        this.Client.Context.ListObjectsAsync(args).Subscribe(item => items.Add(item));
        this.WriteObject(items);
    }
}