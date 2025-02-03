using EduQuest_Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Authenticate.Commands.SignInWithGoogle;

public class SignInGoogleCommand : IRequest<APIResponse>
{
    public string? Token { get; set; }

    public SignInGoogleCommand(string? token)
    {
        Token = token;
    }
}
