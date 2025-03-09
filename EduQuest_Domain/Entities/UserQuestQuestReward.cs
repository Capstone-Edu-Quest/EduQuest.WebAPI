using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities;

public class UserQuestQuestReward
{
    public string? UserQuestId { get; set; }
    public string? QuestRewardId { get; set; }

    public virtual UserQuest UserQuest { get; set; }
    public virtual QuestReward QuestReward { get; set; }
}
