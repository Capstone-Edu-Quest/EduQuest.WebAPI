using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Courses.Query.GetAssignmentAttemptsForIns;

public class GetAssignmentAttemptsForInsQuery : IRequest<APIResponse>
{
    public string courseId { get; set; }

    public GetAssignmentAttemptsForInsQuery(string courseId)
    {
        this.courseId = courseId;
    }
}
