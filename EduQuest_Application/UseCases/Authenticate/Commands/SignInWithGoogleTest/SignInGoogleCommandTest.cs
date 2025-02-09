using EduQuest_Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Authenticate.Commands.SignInWithGoogle;

public class SignInGoogleCommandTest : IRequest<APIResponse>
{
    public string? email { get; set; }

    public SignInGoogleCommandTest(string? email)
    {
        this.email = email;
    }
}
