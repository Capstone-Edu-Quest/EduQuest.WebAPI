
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;


namespace EduQuest_Domain.Entities;

[Table("Report")]
public partial class Report : BaseEntity
{
   
    public DateTime? ClosedAt { get; set; }
    public string Reporter { get; set; } = null!;
    public string Violator { get; set; } = null!;
    public int? Type { get; set; }
    public string Reason { get; set; } = null!;
    public string? FeedbackId { get; set; }
    public string? CourseId { get; set; }
    public int Status { get; set; } // 1 = resolved, 2 = pending, 3 = rejected

    [JsonIgnore]
    public virtual User? User { get; set; }

    [JsonIgnore]
    public virtual Course? Course { get; set; }

    [JsonIgnore]
    public virtual Feedback? Feedback { get; set; }
}
