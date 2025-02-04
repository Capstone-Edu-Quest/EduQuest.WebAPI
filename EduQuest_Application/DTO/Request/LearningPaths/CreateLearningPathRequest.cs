using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request.LearningPaths;

public class CreateLearningPathRequest : IMapFrom<LearningPath>, IMapTo<LearningPath>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsPublic { get; set; }
    public string EstimateDuration { get; set; }
    public List<CreateCourseLearningPath> Courses = new List<CreateCourseLearningPath>();
}
