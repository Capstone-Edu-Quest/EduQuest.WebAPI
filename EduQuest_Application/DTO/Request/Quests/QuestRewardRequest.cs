

using static EduQuest_Domain.Constants.Constants;
using System.ComponentModel.DataAnnotations;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Request.Quests;

public class QuestRewardRequest: IMapFrom<QuestReward>, IMapTo<QuestReward>
{
    [Required(ErrorMessage = MessageError.ValueRequired)]
    public string? RewardType { get; set; }

    [Required(ErrorMessage = MessageError.ValueRequired)]
    public string? RewardValue { get; set; }
}
