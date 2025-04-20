using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Users;

public class UserRankingResponse : IMapFrom<User>
{
    public string Id { get; set; } = string.Empty;
    public string? Username { get; set; }
    public string? AvatarUrl { get; set; }
    public double? score { get; set; }
    public long rank { get; set; }
}
