using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Courses.Query.GetLearnerAssignmentAttempts;

public class GetLearnerAssignmentAttemptsQuery : IRequest<APIResponse>
{
    public string AssignmentId { get; set; }
    public string LessonId { get; set; }

    public GetLearnerAssignmentAttemptsQuery(string assignmentId, string lessonId)
    {
        AssignmentId = assignmentId;
        LessonId = lessonId;
    }
}
