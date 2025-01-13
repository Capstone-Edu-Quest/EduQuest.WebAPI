using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request
{
	public class LearningMaterialStageRequest : IMapFrom<LearningMaterial>, IMapTo<LearningMaterial>
	{
		public string Type { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string UrlMaterial { get; set; }
	}
}
