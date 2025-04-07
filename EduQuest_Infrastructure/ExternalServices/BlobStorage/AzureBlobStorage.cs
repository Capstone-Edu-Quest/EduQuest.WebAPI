using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using EduQuest_Application.Abstractions.AzureBlobStorage;
using EduQuest_Infrastructure.ExternalServices.BlobStorage.Setting;
using Microsoft.Extensions.Options;

namespace EduQuest_Infrastructure.ExternalServices.BlobStorage;

public class AzureBlobStorage : IAzureBlobStorage
{
    private readonly AzureStorageSetting _azureBlobSetting;
    private readonly BlobContainerClient _blobContainerClient;

    public AzureBlobStorage(IOptions<AzureStorageSetting> azureBlobSetting)
    {
        _azureBlobSetting = azureBlobSetting.Value;
        _blobContainerClient = new BlobContainerClient(_azureBlobSetting.ConnectionString, _azureBlobSetting.ContainerName);
        _blobContainerClient.CreateIfNotExists(PublicAccessType.Blob); 
    }


    public BlobContainerClient GetBlobContainerClient()
    {
        return _blobContainerClient;
    }

    // Upload file từ Stream lên Azure Blob Storage
    public async Task UploadAsync(string fileName, Stream fileStream)
    {
        var blobClient = _blobContainerClient.GetBlobClient(fileName);

        await blobClient.UploadAsync(fileStream, overwrite: true);
    }

    // Upload file từ byte[] (ví dụ: file ghép từ chunks)
    public async Task UploadAsync(string fileName, byte[] data)
    {
        var blobClient = _blobContainerClient.GetBlobClient(fileName);
        using var stream = new MemoryStream(data);
        await blobClient.UploadAsync(stream, overwrite: true);
    }

    // Lấy URL của file sau khi upload lên Blob Storage
    public string GetFileUrl(string fileName)
    {
        var blobClient = _blobContainerClient.GetBlobClient(fileName);
        return blobClient.Uri.ToString();
    }
}
