using EduQuest_Application.Abstractions.AzureBlobStorage;
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System.Net;

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
        await _redisCaching.SetAsync(chunkKey, chunkData, 60);

        // Track the number of chunks received for the file
        var receivedChunksKey = $"upload:{fileId}:received";
        var receivedChunks = await _redisCaching.GetAsync<int>(receivedChunksKey);
        if (receivedChunks == 0)
        {
            await _redisCaching.SetAsync(receivedChunksKey, 0, 60);
        }

        await _redisCaching.SetAsync(receivedChunksKey, receivedChunks + 1, 60);

        // Check if all chunks have been uploaded
        if (receivedChunks + 1 == totalChunks)
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

                    // Write the chunk data to the MemoryStream
                    await ms.WriteAsync(chunkDataToRetrieve);
                }

                //upload the assembled video to Azure Blob Storage
                ms.Position = 0; 
                string fileName = $"{fileId}.mp4";
                await _blobStorage.UploadAsync(fileName, ms);

                // After uploading, get the URL of the video
                string fileUrl = _blobStorage.GetFileUrl(fileName);


                // Reset received chunks count in Redis
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
