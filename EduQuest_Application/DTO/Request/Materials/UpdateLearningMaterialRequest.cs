using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request.Materials
{
	public class UpdateLearningMaterialRequest : IMapFrom<Material>, IMapTo<Material>
	{
        public string Id { get; set; }
        public string? Title { get; set; }
		public string? Description { get; set; }
		public VideoRequest? VideoRequest { get; set; }
		public string? Content { get; set; } //Document
		public QuizRequest? QuizRequest { get; set; }
		public AssignmentRequest? AssignmentRequest { get; set; }
	}
}
