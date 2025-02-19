using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities;

public class StudyTime : BaseEntity
{
    public string userId { get; set; }
    public string studyTime { get; set;}
    public DateTime Date { get; set; }

    [JsonIgnore]
    public virtual UserStatistic User { get; set; }
}
