
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System.ComponentModel.DataAnnotations;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.DTO.Request.LearningPaths;

public class UpdateCoursesLearningPath : IMapTo<LearningPathCourse>, IMapFrom<LearningPathCourse>
{
    [Required(ErrorMessage = MessageError.ValueRequired)]
    public string CourseId { get; set; } = string.Empty;

    [Required(ErrorMessage = MessageError.ValueRequired)]
    public int CourseOrder { get; set; }
    public string Action {  get; set; } = string.Empty;// "update", "delete", "add"
}
