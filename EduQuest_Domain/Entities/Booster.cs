using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities;

public partial class Booster : BaseEntity
{
    public double BoostValue { get; set; }
    public DateTime DueDate { get; set; }
    public string UserId { get; set; }

    [JsonIgnore]
    public virtual User? User { get; set; }
}
