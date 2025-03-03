

using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities;

public class QuestReward : BaseEntity
{
    public string? RewardType { get; set; }//thêm bảng phụ 
    public string? RewardValue { get; set; }//thêm bảng phụ
    public string? QuestId { get; set; }



    [JsonIgnore]
    public virtual Quest Quest { get; set; } = null!;
}
