using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Request.Materials
{
    public class CreateMaterialRequest : IMapFrom<Material>, IMapTo<Material>
    {
        //public List<string> StagesId { get; set; }
        public int? Type { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public VideoRequest? Video { get; set; }
		public string? Content { get; set; } //Document
        //public CreateQuizRequest? Quiz { get; set; }
        //public AssignmentRequest? Assignment { get; set; }

    }
}
