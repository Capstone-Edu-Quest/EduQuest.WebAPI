

namespace EduQuest_Application.DTO.Response.Materials;

public class UnreviewedAssignmentAttempt
{
    public string CourseId { get; set; }
    public string CourseName { get; set; }
    public List<AssignmentResponse> Assignments { get; set; } = new List<AssignmentResponse>();
}
