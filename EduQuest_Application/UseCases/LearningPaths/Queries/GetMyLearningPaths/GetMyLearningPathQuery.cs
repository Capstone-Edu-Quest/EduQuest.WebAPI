using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.LearningPaths.Queries.GetMyLearningPaths;

public class GetMyLearningPathQuery: IRequest<APIResponse>
{
    public string UserId { get; set; }
    public string? KeyWord { get; set; }
    public bool? IsPublic { get; set; }
    public bool? IsEnrolled { get; set; }
    public bool? CreatedByExpert { get; set; }
    public int Page {  get; set; }
    public int EachPage { get; set; }

    public GetMyLearningPathQuery(string userId, string? keyWord, bool? isPublic, bool? isEnrolled, 
        bool? createdByExpert, int page, int eachPage)
    {
        UserId = userId;
        KeyWord = keyWord;
        IsPublic = isPublic;
        IsEnrolled = isEnrolled;
        CreatedByExpert = createdByExpert;
        Page = page;
        EachPage = eachPage;
    }
}
