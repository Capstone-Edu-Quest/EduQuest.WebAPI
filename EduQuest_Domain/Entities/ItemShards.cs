

using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities;

public partial class ItemShards : BaseEntity
{
    public string TagId { get; set; }
    public int Quantity { get; set; }
    public string UserId { get; set; }

    [JsonIgnore]
    public virtual User? User { get; set; } = null;
    [JsonIgnore]
    public virtual Tag? Tags { get; set; } = null;
}
