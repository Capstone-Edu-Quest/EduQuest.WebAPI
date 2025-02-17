using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Badges;

public class BadgeDto : IMapFrom<Badge>, IMapTo<Badge>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconUrl { get; set; }
    public string Color { get; set; }
}
