using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Courses.Query.GetAssignmentAttemptsForIns;

public class GetAssignmentAttemptsForInsQuery : IRequest<APIResponse>
{
    public string AssignmentId { get; set; }
    public string LessonId { get; set; }

    public GetAssignmentAttemptsForInsQuery(string assignmentId, string lessonId)
    {
        AssignmentId = assignmentId;
        LessonId = lessonId;
    }
}
