using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Authenticate.Commands.LogOut;

public class SignOutCommand : IRequest<APIResponse>
{
    public string userId { get; set; }
    public string accessToken { get; set; }

}
