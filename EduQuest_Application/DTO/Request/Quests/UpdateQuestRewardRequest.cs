

using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Request.Quests;

public class UpdateQuestRewardRequest : IMapTo<Reward>
{
    public string? Id { get; set; }

    public int? RewardType { get; set; }

    public int? RewardValue { get; set; }
}
