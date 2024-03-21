# Minio PowerShell

Creating Minio Client

```powershell
$Client = Connect-Minio -Endpoint localhost:9000 -Credential (Get-Credential)

# ...
```

## Commands

| Command |
| ------- |
| Connect-Minio |

## Bucket Commands

| Command | 
| ------- |
| Get-MinioBucket |
| Test-MinioBucket |
| New-MinioBucket |
| Update-MinioBucket |
| Remove-MinioBucket |

## Object Commands

| Command |
| ------- |
| Copy-MinioObject |
| Get-MinioObject |
| New-MinioObject |
| Remove-MinioObject |
| Start-MinioDownload |
