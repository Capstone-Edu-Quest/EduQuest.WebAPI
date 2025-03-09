using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Quests;

public class QuestRewardResponse : IMapFrom<QuestReward>
{
    public string? RewardType { get; set; }
    public string? RewardValue { get; set; }
}
