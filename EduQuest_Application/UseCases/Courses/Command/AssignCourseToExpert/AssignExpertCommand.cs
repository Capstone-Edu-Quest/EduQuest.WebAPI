using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Courses.Command.AssignCourseToExpert;

public class AssignExpertCommand : IRequest<APIResponse>
{
    public string CourseId { get; set; }
    public string AssignTo { get; set; }

	public AssignExpertCommand(string courseId, string assignTo)
	{
		CourseId = courseId;
		AssignTo = assignTo;
	}
}
