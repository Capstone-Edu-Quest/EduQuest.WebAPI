using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Boosters;

public class BoosterResponseDto : IMapFrom<Booster>, IMapTo<Booster>
{
    public double? BoostExp { get; set; } = 0.0;
    public double? BoostGold { get; set; } = 0.0;
}
