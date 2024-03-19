using System.Management.Automation;

using Minio.DataModel.Args;

namespace Bal.PsMinio.Management.Commands;

[Cmdlet(VerbsCommon.New, "MinioObject")]
public class NewMinioObjectCommand : BucketOperation
{
    [Parameter]
    public string? Path { get; set; }

    [Parameter]
    public string? ObjectName { get; set; }

    protected override void ProcessRecord()
    {
        string absolutePath = System.IO.Path.GetFullPath(this.SessionState.Path.CurrentFileSystemLocation.Path);

        if (!string.IsNullOrEmpty(this.Path))
        {
            absolutePath = System.IO.Path.GetFullPath(this.Path);
        }

        this.WriteVerbose("Preparing arguments");

        var args = new PutObjectArgs()
            .WithBucket(this.GetBucketName())
            .WithObject(string.IsNullOrEmpty(this.ObjectName) ? System.IO.Path.GetFileName(absolutePath) : this.ObjectName)
            .WithFileName(absolutePath);

        this.WriteVerbose($"Uploading {absolutePath}");

        this.Client.Minio.PutObjectAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();

        this.WriteVerbose("Upload successfully");
    }
}