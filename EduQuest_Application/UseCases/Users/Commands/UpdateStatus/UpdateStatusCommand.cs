using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Users.Commands.UpdateStatus;

public class UpdateStatusCommand : IRequest<APIResponse>
{
    public string UserId { get; set; }
    public string Status { get; set; } = string.Empty;
}
