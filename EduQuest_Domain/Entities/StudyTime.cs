using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities;

public class StudyTime : BaseEntity
{
    public string UserId { get; set; }
    public string StudyTimes { get; set;}
    public DateTime Date { get; set; }

    [JsonIgnore]
    public virtual UserMeta UserMeta { get; set; }
}
