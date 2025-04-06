using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Profiles;

public class InstructorProfileDto : IMapFrom<User>, IMapTo<User>
{
    public string? Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string Headline { get; set; }
    public string Description { get; set; }
    public string AvatarUrl { get; set; }
    public int? TotalLearners { get; set; } = 0;
    public int? TotalReviews { get; set; } = 0;
    public int? AvarageReviews { get; set; } = 0;
    public List<CourseProfileDto> Courses { get; set; }



}
