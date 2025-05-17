using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request.Materials
{
	public class UpdateLessonContentRequest : IMapFrom<Material>, IMapTo<Material>
	{
        public string Id { get; set; }
		public int Type { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public VideoRequest? Video { get; set; }
		public string? Content { get; set; } //Document
		public UpdateQuizRequest? Quiz { get; set; }
		public UpdateAssignmentRequest? Assignment { get; set; }
	}
}
