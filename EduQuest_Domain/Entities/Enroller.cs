using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities;

public class Enroller
{
    public string UserId { get; set; }
    public string LearningPathId { get; set; }

    [JsonIgnore]
    public virtual User User { get; set; }
    [JsonIgnore]
    public virtual LearningPath LearningPath { get; set; }
}
