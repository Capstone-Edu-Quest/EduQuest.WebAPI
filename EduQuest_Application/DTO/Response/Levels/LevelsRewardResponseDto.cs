using AutoMapper;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;

namespace EduQuest_Application.DTO.Response.Levels;

public class LevelsRewardResponseDto : IMapFrom<LevelReward>, IMapTo<LevelReward>
{
    public int? RewardType { get; set; }
    public string? RewardValue { get; set; }
}
