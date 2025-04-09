using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request.Courses;

public class AttemptQuizDto
{
    public string QuizId { get; set; }
    public int TotalTime { get; set; }
    public List<QuestionAnswerDto> Answers { get; set; }

}
