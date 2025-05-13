using EduQuest_Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EduQuest_Application.UseCases.Materials.Command.UploadImage;

public class UploadImageCommand : IRequest<APIResponse>
{
    public IFormFile ImageFile { get; set; }
}
