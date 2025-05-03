using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.LearningPaths;

public class LearningPathPreview : IMapFrom<LearningPath>
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int TotalTimes { get; set; }
    public bool IsPublic { get; set; }
    public DateTime? EnrollDate { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsOverDue { get; set; } = false;
    public bool IsCompleted { get; set; } = false;
    public bool CreatedByExpert { get; set; }
    public CommonUserResponse CreatedBy { get; set; } = new CommonUserResponse();
}
