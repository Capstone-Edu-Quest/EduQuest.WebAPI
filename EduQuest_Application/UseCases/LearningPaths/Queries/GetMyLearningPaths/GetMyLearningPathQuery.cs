using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.LearningPaths.Queries.GetMyLearningPaths;

public class GetMyLearningPathQuery: IRequest<APIResponse>
{
    public string UserId { get; set; }
    public int Page {  get; set; }
    public int EachPage { get; set; }
    public GetMyLearningPathQuery(string userId, int page, int eachPage)
    {
        UserId = userId;
        Page = page;
        EachPage = eachPage;
    }
}
