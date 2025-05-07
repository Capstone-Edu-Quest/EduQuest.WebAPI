using Azure.Storage.Blobs.Models;

namespace EduQuest_Application.Abstractions.AzureBlobStorage;

public interface IAzureBlobStorage
{
    Task UploadAsync(string fileName, Stream stream, BlobHttpHeaders headers);
    Task UploadAsync(string fileName, byte[] data);
    string GetFileUrl(string fileName);
    Task UploadBlockAsync(string blobName, string base64BlockId, Stream blockData);
    Task CommitBlockListAsync(string blobName, IEnumerable<string> base64BlockIds, BlobHttpHeaders headers);
}
