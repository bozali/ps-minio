using Bal.PsMinio.Management.Domain;

using System.Management.Automation;

using Minio.DataModel.Args;

namespace Bal.PsMinio.Management.Commands;

[Cmdlet(VerbsCommon.New, "MinioBucket")]
public class NewMinioBucketCommand : PSCmdlet
{
    [Parameter]
    public MinioClientObject Client { get; set; }

    [ValidateNotNull]
    [Parameter]
    public string Name { get; set; }

    protected override void ProcessRecord()
    {
        var args = new MakeBucketArgs()
            .WithBucket(this.Name);

        this.Client.Minio.MakeBucketAsync(args).ConfigureAwait(true).GetAwaiter().GetResult();

        var response = this.Client.Minio.ListBucketsAsync().ConfigureAwait(true).GetAwaiter().GetResult();

        this.WriteObject(response.Buckets.First(x => x.Name == this.Name));
    }
}