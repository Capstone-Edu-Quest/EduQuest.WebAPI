using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Levels.Query.GetFilterLevels;

public class GetFilterLevelQuery : IRequest<APIResponse>
{
    public int Page { get; set; } = 1;
    public int EachPage { get; set; } = 10;
    public int? LevelNumber { get; set; }
    public int? Exp { get; set; }


    public GetFilterLevelQuery() { }

    public GetFilterLevelQuery(int? page, int? eachPage, int? levelNumber, int? exp)
    {
        Page = page ?? 1;
        EachPage = eachPage ?? 10;
        LevelNumber = levelNumber;
        Exp = exp;
    }
}
