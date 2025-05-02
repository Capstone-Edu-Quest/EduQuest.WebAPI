using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Tags.Queries.GetFilterTag;

public class GetFilterTagQuery : IRequest<APIResponse>
{
    public int? Page { get; set; } = 1;
    public int? EachPage { get; set; } = 10;
    public string? TagId { get; set; }
    public string? Name { get; set; }
	public int? Type { get; set; }

	public GetFilterTagQuery()
    {
    }

	public GetFilterTagQuery(int? page, int? eachPage, string? tagId, string? name, int? type)
	{
		Page = page;
		EachPage = eachPage;
		TagId = tagId;
		Name = name;
		Type = type;
	}
}
