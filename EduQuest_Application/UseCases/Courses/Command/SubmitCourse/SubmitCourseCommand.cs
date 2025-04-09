using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Courses.Command.SubmitCourse;

public class SubmitCourseCommand : IRequest<APIResponse>
{
    public string courseId { get; set; }
}
