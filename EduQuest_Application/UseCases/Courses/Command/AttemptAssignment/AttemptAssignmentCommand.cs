using EduQuest_Application.DTO.Request.Courses;
using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Courses.Command.AttemptAssignment;

public class AttemptAssignmentCommand :IRequest<APIResponse>
{
    public string UserId { get; set; }
    public string LessonId { get; set; }
    public AttemptAssignmentDto Attempt { get; set; }

    public AttemptAssignmentCommand(string userId, string lessonId, AttemptAssignmentDto attempt)
    {
        UserId = userId;
        LessonId = lessonId;
        Attempt = attempt;
    }
}
