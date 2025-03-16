using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Quests;

public class QuestRewardResponse : IMapFrom<Reward>
{
    public string Id { get; set; } = string.Empty;
    public int? RewardType { get; set; }
    public int? RewardValue { get; set; }
}
