using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Users.Commands.SwitchRole;

public class SwitchRoleCommand : IRequest<APIResponse>
{
    public string accessToken { get; set; }
    public string userId { get; set; }
    public string RoleId { get; set; }

}
