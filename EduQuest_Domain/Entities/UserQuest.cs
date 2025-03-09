
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static EduQuest_Domain.Constants.Constants;


namespace EduQuest_Domain.Entities;

public class UserQuest : BaseEntity
{
    public string? Title { get; set; } //change to title?
    public int? Type { get; set; }// daily, one time, dev định nghĩa sẵn trong enum, cố định
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public int? PointToComplete { get; set; }
    public int? CurrentPoint {  get; set; }
    public bool IsCompleted { get; set; }
    public bool? IsDaily { get; set; }
    public string? UserId { get; set; }

    [JsonIgnore]
    public virtual User User { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<QuestReward> Rewards { get; set; }
}
