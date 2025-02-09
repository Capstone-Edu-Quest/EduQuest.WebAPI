using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Users.Queries.GetCurrentUser;

public class GetCurrentUserQuery : IRequest<APIResponse>
{
    public string email { get; set; }

    public GetCurrentUserQuery(string email)
    {
        this.email = email;
    }
}
