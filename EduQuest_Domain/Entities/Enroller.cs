using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities;

public class Enroller : BaseEntity
{
    public string LearningPathId { get; set; }
    public string UserId { get; set; }
    public string CourseId { get; set; }
    public int CourseOrder {  get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsOverDue { get; set; } = false;
    public bool IsCompleted { get; set; } = false;

    [JsonIgnore]
    public virtual LearningPath LearningPath { get; set; }
    [JsonIgnore]
    public virtual User User { get; set; }
    [JsonIgnore]
    public virtual Course Course { get; set; }
}
