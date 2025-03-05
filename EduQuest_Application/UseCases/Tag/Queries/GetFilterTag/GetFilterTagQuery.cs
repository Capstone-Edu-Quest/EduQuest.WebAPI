using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Tag.Queries.GetFilterTag;

public class GetFilterTagQuery : IRequest<APIResponse>
{
    public int? Page { get; set; }
    public int? EachPage { get; set; }
    public string? TagId { get; set; }
    public string? Name { get; set; }

    public GetFilterTagQuery()
    {
    }

    public GetFilterTagQuery(int? page, int? eachPage, string? tagId, string? name)
    {
        Page = page;
        EachPage = eachPage;
        TagId = tagId;
        Name = name;
    }
}
