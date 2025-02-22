using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Users.Commands.UpdateUserRole;

public class UpdateUserRoleCommand : IRequest<APIResponse>
{
    public string UserId { get; set; }
    public int RoleId { get; set; }
}
