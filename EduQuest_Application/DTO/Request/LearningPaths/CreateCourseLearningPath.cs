using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System.ComponentModel.DataAnnotations;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.DTO.Request.LearningPaths;

public class CreateCourseLearningPath : IMapFrom<LearningPathCourse>, IMapTo<LearningPathCourse>
{
    [Required(ErrorMessage = MessageError.ValueRequired)]
    public string CourseId { get; set; }

    [Required(ErrorMessage = MessageError.ValueRequired)]
    public int CourseOrder { get; set; }
}
