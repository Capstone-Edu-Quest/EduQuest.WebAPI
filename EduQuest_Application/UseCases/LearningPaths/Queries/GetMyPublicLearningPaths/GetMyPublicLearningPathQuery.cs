using EduQuest_Domain.Models.Response;
using MediatR;


namespace EduQuest_Application.UseCases.LearningPaths.Queries.GetMyPublicLearningPaths;

public class GetMyPublicLearningPathQuery: IRequest<APIResponse>
{
    public string? UserId { get; set; }
    public string? KeyWord { get; set; }
    public GetMyPublicLearningPathQuery(string? userId, string? keyWord)
    {
        UserId = userId;
        KeyWord = keyWord;
    }
}
