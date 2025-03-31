using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Authenticate.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest<APIResponse>
{
    public string Email { get; set; }
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
}
