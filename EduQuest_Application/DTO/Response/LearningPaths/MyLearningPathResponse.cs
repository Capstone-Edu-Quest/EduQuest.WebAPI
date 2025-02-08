using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response.LearningPaths;

public class MyLearningPathResponse : IMapFrom<LearningPath>
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int TotalTimes { get; set; }
    public int TotalCourses {  get; set; }
    public bool IsPublic { get; set; }
    public bool IsEnrolled { get; set; }
    public CommonUserResponse CreatedBy { get; set; } = new CommonUserResponse();
}
