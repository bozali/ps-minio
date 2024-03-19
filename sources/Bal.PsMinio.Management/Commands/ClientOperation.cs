using System.Management.Automation;

using Bal.PsMinio.Management.Domain;

namespace Bal.PsMinio.Management.Commands;

public class ClientOperation : PSCmdlet
{
    [Parameter]
    public MinioClientObject Client { get; set; }
}