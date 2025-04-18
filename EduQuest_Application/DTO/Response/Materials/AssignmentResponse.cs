using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Materials;

public class AssignmentResponse :IMapFrom<Assignment>
{
    public string Id { get; set; }
    public double? TimeLimit { get; set; }
    public string? Question { get; set; }
    public string? AnswerLanguage { get; set; }
    public string? ExpectedAnswer { get; set; }
    public List<AssignmentAttemptResponseForInstructor> attempts { get; set; } = new List<AssignmentAttemptResponseForInstructor>();
}
