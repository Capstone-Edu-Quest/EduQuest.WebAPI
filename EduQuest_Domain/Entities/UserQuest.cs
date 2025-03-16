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
    public string? QuestValues { get; set; }
    public int PointToComplete { get; set; }
    public int? CurrentPoint {  get; set; }
    public bool IsCompleted { get; set; }
    public string? UserId { get; set; }
    public string? QuestId { get; set; }

    [JsonIgnore]
    public virtual User User { get; set; } = null!;

	[JsonIgnore]
	public virtual UserQuestReward UserQuestReward { get; set; }
}
