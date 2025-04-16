using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Courses.Query.GetQuizAttempts;

public class GetQuizAttemptsQuery :IRequest<APIResponse>
{
    public string QuizId { get; set; }
    public string LessonId { get; set; }
    public string UserId { get; set; }

    public GetQuizAttemptsQuery(string quizId, string lessonId, string userId)
    {
        QuizId = quizId;
        LessonId = lessonId;
        UserId = userId;
    }
}
