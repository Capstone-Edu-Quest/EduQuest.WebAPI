using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request.Courses;

public class GradingAssignmentDto
{
    public string AssignmentAttemptId { get; set; }
    public int Grade { get; set; }
    public string? Comment { get; set; }
}
