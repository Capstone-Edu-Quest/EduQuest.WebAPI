using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Authenticate.Commands.ResetPassword;

public class ResetPasswordCommand : IRequest<APIResponse>
{
    public string? Email { get; set; }
}
