using EduQuest_Application.DTO.Request.Courses;
using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Courses.Command.AttemptQuiz;

public class AttemptQuizCommand : IRequest<APIResponse>
{
    public string UserId { get; set; }
    public string LessonId { get; set; }
    public AttemptQuizDto Attempt {  get; set; }

    public AttemptQuizCommand(string userId, string lessonId, AttemptQuizDto attempt)
    {
        UserId = userId;
        LessonId = lessonId;
        Attempt = attempt;
    }
}
