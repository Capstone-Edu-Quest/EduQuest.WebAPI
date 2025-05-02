using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Tags.Commands.CreateTag;

public class CreateTagCommand : IRequest<APIResponse>
{
    public string? TagName { get; set; }
	public int? Level { get; set; }
	public int? Grade { get; set; }
	public int? Type { get; set; }

	public CreateTagCommand(string? tagName, int? level, int? grade, int? type)
	{
		TagName = tagName;
		Level = level;
		Grade = grade;
		Type = type;
	}
}