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
		public int? Type { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public string? UrlMaterial { get; set; }
		public int? EstimateTime { get; set; }
	}
}
