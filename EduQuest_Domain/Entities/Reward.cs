

using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities;

public class Reward : BaseEntity
{
    public int? RewardType { get; set; }//thêm bảng phụ 
    public int? RewardValue { get; set; }//thêm bảng phụ

    public string? QuestId { get; set; }
    //public string? UserQuestId { get; set; }

    [JsonIgnore]
    public virtual Quest Quest { get; set; }

	[JsonIgnore]
	public virtual UserQuestReward UserQuestReward { get; set; }
}
