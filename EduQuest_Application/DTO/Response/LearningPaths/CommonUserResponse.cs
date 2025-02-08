using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.LearningPaths;

public class CommonUserResponse : IMapFrom<User>
{
    public string Id { get; set; } = string.Empty;
    public string? Username { get; set; }
    //public string? Email { get; set; }
    public string? AvatarUrl { get; set; }
}
