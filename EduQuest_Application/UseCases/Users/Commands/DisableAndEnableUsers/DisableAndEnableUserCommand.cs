

using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Users.Commands.DisableAndEnableUsers;

public class DisableAndEnableUserCommand : IRequest<APIResponse>
{
    public string UserId { get; set; }
    public string AdminId { get; set; }

    public DisableAndEnableUserCommand(string userId, string adminId)
    {
        UserId = userId;
        AdminId = adminId;
    }
}
