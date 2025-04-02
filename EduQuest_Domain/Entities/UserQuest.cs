using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace EduQuest_Domain.Entities;
[Table("UserQuest")]
public class UserQuest : BaseEntity
{
    public string? Title { get; set; } //change name to title
    public int? Type { get; set; }// daily, one time, dev định nghĩa sẵn trong enum, cố định
    public int? QuestType { get; set; }// learning streak, complete courses,....
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public string? QuestValues { get; set; } //break to array when response to fe.
    public string? RewardTypes { get; set; } //break to array when response to fe.
    public string? RewardValues { get; set; } //break to array when response to fe.
    public int PointToComplete { get; set; }
    public int? CurrentPoint {  get; set; }
    public bool IsCompleted { get; set; }
    public string? UserId { get; set; }
    public string? QuestId { get; set; }
    public bool IsRewardClaimed { get; set; } = false;

    [JsonIgnore]
    public virtual User User { get; set; } = null!;

}
