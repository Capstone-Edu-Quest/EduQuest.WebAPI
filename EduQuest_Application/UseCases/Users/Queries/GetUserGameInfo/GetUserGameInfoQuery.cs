using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Users.Queries.GetUserGameInfo;

public class GetUserGameInfoQuery : IRequest<APIResponse>
{
    public string userId { get; set; }


    public GetUserGameInfoQuery(string userId)
    {
        this.userId = userId;
    }
}
