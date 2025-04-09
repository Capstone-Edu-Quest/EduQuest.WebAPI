using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities;


[Table("AssignmentReviews")]
public partial class AssignmentPeerReview: BaseEntity
{
    public string AssignmentAttemptId { get; set; }
    public string ReviewerId { get; set; }
    public string Score { get; set; }
    public string? Comment { get; set; }

    [JsonIgnore]
    public virtual AssignmentAttempt AssignmentAttempt { get; set; }
}
