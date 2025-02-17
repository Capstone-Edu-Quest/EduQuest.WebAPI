using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Request.Materials
{
    public class CreateLearningMaterialRequest : IMapFrom<LearningMaterial>, IMapTo<LearningMaterial>
    {
        public List<string> StagesId { get; set; }
        public int? Type { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? UrlMaterial { get; set; }
        public int? EstimateTime { get; set; }
    }
}
