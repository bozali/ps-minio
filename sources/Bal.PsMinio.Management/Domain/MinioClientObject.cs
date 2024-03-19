using Minio;

namespace Bal.PsMinio.Management.Domain;

public class MinioClientObject
{
    internal MinioClientObject(IMinioClient client)
    {
        this.Minio = client;
    }

    internal IMinioClient Minio { get; set; }
}