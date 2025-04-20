using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Users.Queries.GetMyInstructorApplicationQuery;

public class GetMyInstructorApplicationQuery : IRequest<APIResponse>
{
    public string userId { get; set; }

    public GetMyInstructorApplicationQuery(string userId)
    {
        this.userId = userId;
    }
}
