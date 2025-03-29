using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Authenticate.Commands.SignInWithPassword;

public class SignInWithPassword : IRequest<APIResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }

}
