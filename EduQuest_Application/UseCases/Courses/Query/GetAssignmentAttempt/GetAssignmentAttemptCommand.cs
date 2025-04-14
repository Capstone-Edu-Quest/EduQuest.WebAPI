using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Courses.Query.GetAssignmentAttempt;

public class GetAssignmentAttemptCommand : IRequest<APIResponse>
{
    public string UserId { get; set; }
    public string AssignmentId { get; set; }
    public string LessonId { get; set; }

    public GetAssignmentAttemptCommand(string userId, string assignmentId, string lessonId)
    {
        UserId = userId;
        AssignmentId = assignmentId;
        LessonId = lessonId;
    }
}
