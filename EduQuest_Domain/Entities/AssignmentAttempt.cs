using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities;


[Table("AssignmentAttempts")]
public partial class AssignmentAttempt : BaseEntity
{
    public string AssignmentId { get; set; }
    public string LessonId { get; set; }
    public string UserId { get; set; }
    public int AttemptNo { get; set; }
    public string AnswerContent { get; set; }
    public double ToTalTime { get; set; }
    public double AnswerScore { get; set; }// average score of total Peer review


    [JsonIgnore]
    public virtual Assignment Assignment { get; set; }
    [JsonIgnore]
    public virtual User User { get; set; }
    [JsonIgnore]
    public virtual Lesson Lesson { get; set; }
    [JsonIgnore]
    public virtual ICollection<AssignmentPeerReview> Reviewers { get; set; }
}
