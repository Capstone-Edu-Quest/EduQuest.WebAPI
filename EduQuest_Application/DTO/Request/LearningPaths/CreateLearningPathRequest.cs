using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System.ComponentModel.DataAnnotations;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.DTO.Request.LearningPaths;

public class CreateLearningPathRequest : IMapFrom<LearningPath>, IMapTo<LearningPath>
{
    [Required(ErrorMessage =MessageError.NameIsRequired)]
    [MaxLength(255, ErrorMessage = "MAX_LENGTH_NAME")]
    public string Name { get; set; }

    [Required(ErrorMessage = MessageError.DescriptionRequired)]
    [MaxLength(1500, ErrorMessage = "MAX_LENGTH_DESCRIPTION")]
    public string Description { get; set; }

    [Required(ErrorMessage = MessageError.ValueRequired)]
    public bool IsPublic { get; set; }

    [Required(ErrorMessage = MessageError.ValueRequired)]
    [MaxLength(50, ErrorMessage = "MAX_LENGTH_DURATION")]
    public string EstimateDuration { get; set; }

    [Required(ErrorMessage = MessageError.ValueRequired)]
    public List<CreateCourseLearningPath> Courses { get; set; } = new List<CreateCourseLearningPath>();

}
