using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response.Courses;

public class CourseLearnerResponse : IMapFrom<CourseLearner>
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string CourseId { get; set; }
    public bool IsActive { get; set; }
    public string? CurrentLessonId { get; set; }
    public int CurrentContentIndex { get; set; } = 0;
    public double? TotalTime { get; set; }
    public double? ProgressPercentage { get; set; }
    public int? ItemShard { get; set; } = null;
    public Dictionary<string, int>? ItemShards { get; set; }
}
