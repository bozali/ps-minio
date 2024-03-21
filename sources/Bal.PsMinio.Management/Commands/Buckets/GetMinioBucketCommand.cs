using System.Management.Automation;

using Bal.PsMinio.Management.Domain;

using Minio.DataModel;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Bal.PsMinio.Management.Commands.Buckets;

[Cmdlet(VerbsCommon.Get, "MinioBucket")]
public class GetMinioBucketCommand : PSCmdlet
{
    [Parameter]
    public MinioClientObject Client { get; set; }

    [Parameter]
    public string[]? Name { get; set; }

    protected override void ProcessRecord()
    {
        var response = this.Client.Context.ListBucketsAsync().GetAwaiter().GetResult();

        if (this.Name?.Any() == true)
        {
            var buckets = new List<Bucket>();

            foreach (string name in this.Name)
            {
                var wildcard = new WildcardPattern(name, WildcardOptions.IgnoreCase | WildcardOptions.Compiled);

                buckets.AddRange(response.Buckets.Where(x => wildcard.IsMatch(x.Name)).Except(buckets));
            }

            this.WriteObject(buckets);
            return;
        }

        this.WriteObject(response.Buckets);
    }
}