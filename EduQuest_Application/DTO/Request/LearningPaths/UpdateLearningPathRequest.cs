
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System.ComponentModel.DataAnnotations;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.DTO.Request.LearningPaths;

public class UpdateLearningPathRequest : IMapTo<LearningPath>
{
    
    [MaxLength(255, ErrorMessage = "MAX_LENGTH_NAME")]
    public string? Name { get; set; } = string.Empty;

   
    [MaxLength(2500, ErrorMessage = "MAX_LENGTH_DESCRIPTION")]
    public string? Description { get; set; } = string.Empty;

    public bool? IsPublic { get; set; }
    public List<UpdateCoursesLearningPath> Courses { get; set; } = new List<UpdateCoursesLearningPath>();
}
