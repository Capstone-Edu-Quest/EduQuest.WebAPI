using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response.Courses;

public class AssignmentAttemptResponseUser : IMapFrom<AssignmentAttempt>
{
    public string Id { get; set; }
    public string AssignmentId { get; set; }
    public string LessonId { get; set; }
    public string UserId { get; set; }
    public int AttemptNo { get; set; }
    public string AnswerContent { get; set; }
    public double ToTalTime { get; set; }
    public double AnswerScore { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public int? ItemShard { get; set; } = null;
    public Dictionary<string, int>? ItemShards { get; set; }
}
