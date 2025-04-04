using EduQuest_Application.DTO.Response.Lessons;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Profiles;

public class CourseInstructorProfileDto : IMapFrom<Course>, IMapTo<Course>
{
    public string Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Author { get; set; }
    public string? PhotoUrl { get; set; }
    public decimal Price { get; set; }
    public double? Rating { get; set; } = 0;
}
