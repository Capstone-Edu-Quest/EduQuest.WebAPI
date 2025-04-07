using EduQuest_Application.Abstractions.AzureBlobStorage;
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System.Net;
using StackExchange.Redis;

namespace EduQuest_Application.UseCases.Materials.Command.UploadVideo;

public class UploadVideoCommandHandler : IRequestHandler<UploadVideoCommand, APIResponse>
{
    private readonly IRedisCaching _redisCaching;
    private readonly IAzureBlobStorage _blobStorage;
    private readonly IMaterialRepository _materialRepository;

    public UploadVideoCommandHandler(IRedisCaching redisCaching, IAzureBlobStorage blobStorage, IMaterialRepository materialRepository)
    {
        _redisCaching = redisCaching;
        _blobStorage = blobStorage;
        _materialRepository = materialRepository;
    }

    public async Task<APIResponse> Handle(UploadVideoCommand request, CancellationToken cancellationToken)
    {
        string fileId = request.FileId;
        int totalChunks = request.TotalChunks;
        int chunkIndex = request.ChunkIndex;
        byte[] chunkData = request.ChunkData;

        var chunkKey = $"upload:{fileId}:chunk:{chunkIndex}";
        await _redisCaching.SetAsync(chunkKey, chunkData, 120);

        // Sử dụng Hash để theo dõi các chunks đã nhận
        var receivedChunksKey = $"upload:{fileId}:chunks";

        // Thêm chunk hiện tại vào hash tracking
        var hashEntries = new HashEntry[]
        {
            new HashEntry(chunkIndex.ToString(), "1")
        };

        await _redisCaching.HashSetAsync(receivedChunksKey, hashEntries, 120);

        // Lấy tất cả hash data để đếm số chunks đã nhận
        var chunksReceived = await _redisCaching.GetAllHashDataAsync(receivedChunksKey);
        int receivedChunksCount = chunksReceived?.Count ?? 0;

        // Kiểm tra nếu tất cả chunks đã được upload
        if (receivedChunksCount >= totalChunks)
        {
            using (var ms = new MemoryStream())
            {
                for (int i = 0; i < totalChunks; i++)
                {
                    var chunkKeyToRetrieve = $"upload:{fileId}:chunk:{i}";
                    var chunkDataToRetrieve = await _redisCaching.GetAsync<byte[]>(chunkKeyToRetrieve);
                    if (chunkDataToRetrieve == null)
                    {
                        return GeneralHelper.CreateErrorResponse(
                            HttpStatusCode.OK,
                            $"Missing chunk {i} for file {fileId}. Retry upload.",
                            $"Missing chunk {i} for file {fileId}. Retry upload.",
                            "name",
                            "file");
                    }

                    await ms.WriteAsync(chunkDataToRetrieve);
                }

                // Upload video đã được ghép vào Azure Blob Storage
                ms.Position = 0;
                string fileName = $"{fileId}.mp4";
                await _blobStorage.UploadAsync(fileName, ms);

                string fileUrl = _blobStorage.GetFileUrl(fileName);

                for (int i = 0; i < totalChunks; i++)
                {
                    await _redisCaching.DeleteKeyAsync($"upload:{fileId}:chunk:{i}");
                }

                await _redisCaching.DeleteKeyAsync(receivedChunksKey);

                return GeneralHelper.CreateSuccessResponse(
                    HttpStatusCode.OK,
                    "Video uploaded successfully",
                    new
                    {
                        url = fileUrl
                    },
                    "name",
                    "file"
                );
            }
        }

        return GeneralHelper.CreateSuccessResponse(
            HttpStatusCode.OK,
            $"Chunk {chunkIndex} uploaded. Waiting for more chunks...",
            null,
            "name",
            "Material"
        );
    }
}