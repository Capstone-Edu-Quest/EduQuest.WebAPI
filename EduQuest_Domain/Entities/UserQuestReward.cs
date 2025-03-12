using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities;

public class UserQuestReward : BaseEntity
{
    public string? UserQuestId { get; set; }
    public string? RewardId { get; set; }

    public virtual ICollection<UserQuest> UserQuests{ get; set; }
    public virtual ICollection<Reward> Rewards { get; set; }
}
