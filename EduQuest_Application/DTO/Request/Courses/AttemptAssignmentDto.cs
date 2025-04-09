using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request.Courses;

public class AttemptAssignmentDto
{
    public string AssignmentId { get; set; }
    public double TotalTime { get; set; }
    public string AnswerContent { get; set; }
}
