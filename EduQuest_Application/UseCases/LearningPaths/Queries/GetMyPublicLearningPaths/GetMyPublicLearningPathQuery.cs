using EduQuest_Domain.Models.Response;
using MediatR;


namespace EduQuest_Application.UseCases.LearningPaths.Queries.GetMyPublicLearningPaths;

public class GetMyPublicLearningPathQuery: IRequest<APIResponse>
{
    public string? UserId { get; set; }
    public string? KeyWord { get; set; }
    public string? CurrentUserId { get; set; }
    public GetMyPublicLearningPathQuery(string? userId, string? keyWord, string? currentUserId)
    {
        UserId = userId;
        KeyWord = keyWord;
        CurrentUserId = currentUserId;
    }
}
