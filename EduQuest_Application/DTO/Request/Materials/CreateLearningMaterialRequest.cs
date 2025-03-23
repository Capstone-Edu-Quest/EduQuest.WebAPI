using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Request.Materials
{
    public class CreateLearningMaterialRequest : IMapFrom<Material>, IMapTo<Material>
    {
        public int? Type { get; set; }
        public VideoRequest? VideoRequest { get; set; }
		public string? Content { get; set; } //Document
        public QuizRequest? QuizRequest { get; set; }
        public AssignmentRequest? AssignmentRequest { get; set; }

    }
}
