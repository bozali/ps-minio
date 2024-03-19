using System.Management.Automation;
using System.Net;

using Bal.PsMinio.Management.Domain;
using Bal.PsMinio.Management.Extensions;

using Minio;

namespace Bal.PsMinio.Management.Commands;

[Cmdlet(VerbsCommunications.Connect, "Minio")]
public class ConnectMinioCommand : PSCmdlet
{
    [Parameter]
    public string Endpoint { get; set; }

    [Parameter(ParameterSetName = "Credential")]
    public string? User { get; set; }

    [Parameter]
    public int? Timeout { get; set; }

    [Parameter(ParameterSetName = "Credential")]
    public string? Password { get; set; }

    [Parameter(ParameterSetName = "PSCredential")]
    public PSCredential? Credential { get; set; }

    [Parameter]
    public WebProxy? Proxy { get; set; }

    [Parameter]
    public bool UseSsl { get; set; }

    protected override void ProcessRecord()
    {
        string endpoint = string.IsNullOrEmpty(this.Endpoint) ? "localhost:9000" : this.Endpoint;

        var client = new MinioClient().WithEndpoint(endpoint);

        if (string.Equals(this.ParameterSetName, "PSCredential", StringComparison.OrdinalIgnoreCase))
        {
            client = client.WithCredentials(this.Credential?.UserName, this.Credential?.Password.ConvertToString());
        }
        else if (string.Equals(this.ParameterSetName, "Credential", StringComparison.OrdinalIgnoreCase))
        {
            client.WithCredentials(this.User, this.Password);
        }

        if (this.Timeout.HasValue)
        {
            client = client.WithTimeout(this.Timeout.Value);
        }

        if (this.Proxy != null)
        {
            client = client.WithProxy(this.Proxy);
        }

        if (this.UseSsl)
        {
            client = client.WithSSL();
        }

        var result = new MinioClientObject(client.Build());

        this.WriteObject(result);
    }
}