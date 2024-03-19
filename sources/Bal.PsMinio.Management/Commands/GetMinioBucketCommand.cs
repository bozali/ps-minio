using System.Management.Automation;

using Bal.PsMinio.Management.Domain;

using Minio.DataModel;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Bal.PsMinio.Management.Commands;

[Cmdlet(VerbsCommon.Get, "MinioBucket")]
public class GetMinioBucketCommand : PSCmdlet
{
    [Parameter]
    public MinioClientObject Client { get; set; }

    [Parameter]
    public string[] Name { get; set; }

    protected override void ProcessRecord()
    {
        var buckets = new List<Bucket>();

        var response = this.Client.Minio.ListBucketsAsync().ConfigureAwait(true).GetAwaiter().GetResult();

        if (this.Name?.Any() == true)
        {
            foreach (string name in this.Name)
            {
                buckets.AddRange(response.Buckets.Where(x => x.Name.ToLowerInvariant().Contains(name)));
            }
        }
        else
        {
            buckets.AddRange(response.Buckets);
        }

        this.WriteObject(buckets);
    }
}