using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Tags.Queries.GetFilterTag;

public class GetFilterTagQuery : IRequest<APIResponse>
{
    public int? Page { get; set; } = 1;
    public int? EachPage { get; set; } = 10;
    public string? TagId { get; set; }
    public string? Name { get; set; }

    public GetFilterTagQuery()
    {
    }

    public GetFilterTagQuery(int? page, int? eachPage, string? tagId, string? name)
    {
        Page = page ?? 1;
        EachPage = eachPage ?? 10;
        TagId = tagId;
        Name = name;
    }
}
