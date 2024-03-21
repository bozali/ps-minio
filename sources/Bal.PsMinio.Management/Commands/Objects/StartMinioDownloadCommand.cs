using System.Management.Automation;
using Minio.DataModel.Args;

namespace Bal.PsMinio.Management.Commands.Objects;

[Cmdlet(VerbsLifecycle.Start, "MinioDownload")]
public class StartMinioDownloadCommand : BucketOperation
{
    [Parameter(Mandatory = true)]
    public string Object { get; set; }

    [Parameter(Mandatory = true)]
    public string Path { get; set; }

    protected override void ProcessRecord()
    {
        string bucketName = this.GetBucketName();

        using var memory = new MemoryStream();

        var getArgs = new GetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(this.Object)
            // ReSharper disable once AccessToDisposedClosure
            .WithCallbackStream(stream => stream.CopyTo(memory));

        var response = this.Client.Context.GetObjectAsync(getArgs).GetAwaiter().GetResult();

        memory.Seek(0, SeekOrigin.Begin);

        using var stream = new FileStream(this.Path, FileMode.Create);
        memory.CopyTo(stream);
    }
}