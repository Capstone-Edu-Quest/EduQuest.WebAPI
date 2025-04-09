using Azure.Storage.Blobs.Models;

namespace EduQuest_Application.Abstractions.AzureBlobStorage;

public interface IAzureBlobStorage
{
    Task UploadAsync(string fileName, Stream stream, BlobHttpHeaders headers);
    Task UploadAsync(string fileName, byte[] data);
    string GetFileUrl(string fileName);
}
