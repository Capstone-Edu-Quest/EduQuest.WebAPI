

using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities;

public class Reward : BaseEntity
{
    public string? RewardType { get; set; }//thêm bảng phụ 
    public string? RewardValue { get; set; }//thêm bảng phụ

    public string? QuestId { get; set; }
    //public string? UserQuestId { get; set; }

    [JsonIgnore]
    public virtual Quest Quest { get; set; }

	[JsonIgnore]
	public virtual UserQuestReward UserQuestReward { get; set; }
}
