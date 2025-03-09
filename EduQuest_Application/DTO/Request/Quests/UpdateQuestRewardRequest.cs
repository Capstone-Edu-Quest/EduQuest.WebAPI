

using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Request.Quests;

public class UpdateQuestRewardRequest : IMapTo<QuestReward>
{
    public string? Id { get; set; }

    public string? RewardType { get; set; }

    public string? RewardValue { get; set; }
}
