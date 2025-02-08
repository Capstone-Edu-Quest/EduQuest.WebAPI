

using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.LearningPaths;

public class LearningPathCourseResponse : IMapFrom<Course>
{
    public string Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Color { get; set; }
    public decimal? Price { get; set; }
    public int Order { get; set; }
}
