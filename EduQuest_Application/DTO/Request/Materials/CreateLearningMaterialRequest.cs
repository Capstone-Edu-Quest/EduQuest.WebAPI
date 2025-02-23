using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Request.Materials
{
    public class CreateLearningMaterialRequest : IMapFrom<LearningMaterial>, IMapTo<LearningMaterial>
    {
        //public List<string> StagesId { get; set; }
        public int? Type { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public VideoRequest? VideoRequest { get; set; }
		public string? Content { get; set; } //Document
        public QuizRequest? QuizRequest { get; set; }
        public AssignmentRequest? AssignmentRequest { get; set; }

    }
}
