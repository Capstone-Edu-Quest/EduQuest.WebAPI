namespace EduQuest_Application.Abstractions.AzureBlobStorage;

public interface IAzureBlobStorage
{
    Task UploadAsync(string fileName, Stream fileStream);
    Task UploadAsync(string fileName, byte[] data);
    string GetFileUrl(string fileName);
}
