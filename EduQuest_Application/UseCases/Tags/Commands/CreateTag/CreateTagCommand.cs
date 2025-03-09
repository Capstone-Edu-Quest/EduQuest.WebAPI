using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Tags.Commands.CreateTag;

public class CreateTagCommand : IRequest<APIResponse>
{
    public string? TagName { get; set; }
}