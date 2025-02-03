using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Request
{
	public class LearningMaterialStageRequest : IMapFrom<LearningMaterial>, IMapTo<LearningMaterial>
	{
		public int? Type { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public string? UrlMaterial { get; set; }
		public int? EstimateTime { get; set; }
	}
}
