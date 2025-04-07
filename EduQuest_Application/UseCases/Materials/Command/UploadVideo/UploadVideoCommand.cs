using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Materials.Command.UploadVideo;

public class UploadVideoCommand : IRequest<APIResponse>
{
    public string FileId { get; set; }
    public byte[] ChunkData { get; set; }
    public int ChunkIndex { get; set; }
    public int TotalChunks { get; set; }
}
