using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.LearningPaths;

public class LearningPathTagResponse : IMapFrom<Tag>
{
    public string Id { get; set; }
    public string Name { get; set; }
}
