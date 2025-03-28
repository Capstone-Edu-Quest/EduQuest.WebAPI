namespace EduQuest_Domain.Entities;

public class LevelReward : BaseEntity
{
    public string? LevelId { get; set; }
    public virtual Level Level { get; set; }

    public int? RewardType { get; set; }
    public string? RewardValue { get; set; }
}
