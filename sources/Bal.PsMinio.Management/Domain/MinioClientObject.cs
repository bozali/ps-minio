using Minio;

namespace Bal.PsMinio.Management.Domain;

public class MinioClientObject
{
    internal MinioClientObject(IMinioClient client)
    {
        this.Context = client;
    }

    internal IMinioClient Context { get; set; }
}