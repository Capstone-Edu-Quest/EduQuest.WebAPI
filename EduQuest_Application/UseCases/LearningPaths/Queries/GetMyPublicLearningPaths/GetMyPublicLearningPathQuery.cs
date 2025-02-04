using EduQuest_Domain.Models.Response;
using MediatR;


namespace EduQuest_Application.UseCases.LearningPaths.Queries.GetMyPublicLearningPaths;

public class GetMyPublicLearningPathQuery: IRequest<APIResponse>
{
    public string UserId { get; set; }

    public GetMyPublicLearningPathQuery(string userId)
    {
        UserId = userId;
    }
}
