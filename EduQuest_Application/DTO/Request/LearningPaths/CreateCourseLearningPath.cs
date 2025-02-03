using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request.LearningPaths;

public class CreateCourseLearningPath : IMapFrom<LearningPathCourse>, IMapTo<LearningPathCourse>
{
    public string CourseId { get; set; }
    public int CourseOrder { get; set; }
}
