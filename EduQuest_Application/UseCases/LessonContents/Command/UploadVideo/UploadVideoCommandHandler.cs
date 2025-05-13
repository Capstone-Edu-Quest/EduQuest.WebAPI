using EduQuest_Application.Abstractions.AzureBlobStorage;
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System.Net;
using StackExchange.Redis;
using Azure.Storage.Blobs.Models;
using System.Text;

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


        string blobName = $"{fileId}.mp4";
        string blockId = Convert.ToBase64String(Encoding.UTF8.GetBytes(chunkIndex.ToString("D6")));

        using (var stream = new MemoryStream(chunkData))
        {
            await _blobStorage.UploadBlockAsync(blobName, blockId, stream);
        }

        string redisBlockListKey = $"upload:{fileId}:blockIds";
        await _redisCaching.HashSetAsync(redisBlockListKey, new HashEntry[]
        {
        new HashEntry(chunkIndex.ToString(), blockId)
        }, 60);

        var allBlocks = await _redisCaching.GetAllHashDataAsync(redisBlockListKey);
        if (allBlocks?.Count >= totalChunks)
        {
            var sortedBlockIds = allBlocks
                .OrderBy(a => int.Parse(a.Key))
                .Select(a => a.Value.ToString())
                .ToList();

            var headers = new BlobHttpHeaders { ContentType = "video/mp4" };
            await _blobStorage.CommitBlockListAsync(blobName, sortedBlockIds, headers);

            await _redisCaching.DeleteKeyAsync(redisBlockListKey);

            string fileUrl = _blobStorage.GetFileUrl(blobName);

            return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, "Video uploaded successfully", new { url = fileUrl }, "name", "file");
        }

        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, $"Chunk {chunkIndex} uploaded. Waiting for more chunks...", null, "name", "Material");

        //    var chunkKey = $"upload:{fileId}:chunk:{chunkIndex}";
        //    await _redisCaching.SetAsync(chunkKey, chunkData, 120);

        //    // using HashSet data structure to track chunks
        //    var receivedChunksKey = $"upload:{fileId}:chunks";

        //    var hashEntries = new HashEntry[]
        //    {
        //        new HashEntry(chunkIndex.ToString(), "1")
        //    };

        //    await _redisCaching.HashSetAsync(receivedChunksKey, hashEntries, 120);

        //    // Get all data in HashSet with key
        //    var chunksReceived = await _redisCaching.GetAllHashDataAsync(receivedChunksKey);
        //    int receivedChunksCount = chunksReceived?.Count ?? 0;

        //    // Count whether all the chunks is stored
        //    if (receivedChunksCount >= totalChunks)
        //    {
        //        using (var ms = new MemoryStream())
        //        {
        //            for (int i = 0; i < totalChunks; i++)
        //            {
        //                var chunkKeyToRetrieve = $"upload:{fileId}:chunk:{i}";
        //                var chunkDataToRetrieve = await _redisCaching.GetAsync<byte[]>(chunkKeyToRetrieve);
        //                if (chunkDataToRetrieve == null)
        //                {
        //                    return GeneralHelper.CreateErrorResponse(
        //                        HttpStatusCode.OK,
        //                        $"Missing chunk {i} for file {fileId}. Retry upload.",
        //                        $"Missing chunk {i} for file {fileId}. Retry upload.",
        //                        "name",
        //                        "file");
        //                }

        //                await ms.WriteAsync(chunkDataToRetrieve);
        //            }


        //            ms.Position = 0;
        //            string fileName = $"{fileId}.mp4";
        //            var blobHttpHeaders = new BlobHttpHeaders
        //            {
        //                ContentType = "video/mp4"
        //            };

        //            await _blobStorage.UploadAsync(fileName, ms, blobHttpHeaders);


        //            string fileUrl = _blobStorage.GetFileUrl(fileName);

        //            for (int i = 0; i < totalChunks; i++)
        //            {
        //                await _redisCaching.DeleteKeyAsync($"upload:{fileId}:chunk:{i}");
        //            }

        //            await _redisCaching.DeleteKeyAsync(receivedChunksKey);

        //            return GeneralHelper.CreateSuccessResponse(
        //                HttpStatusCode.OK,
        //                "Video uploaded successfully",
        //                new
        //                {
        //                    url = fileUrl
        //                },
        //                "name",
        //                "file"
        //            );
        //        }
        //    }

        //    return GeneralHelper.CreateSuccessResponse(
        //        HttpStatusCode.OK,
        //        $"Chunk {chunkIndex} uploaded. Waiting for more chunks...",
        //        null,
        //        "name",
        //        "Material"
        //    );
        //}
    }
}