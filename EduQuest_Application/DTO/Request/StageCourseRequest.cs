using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request
{
	public class StageCourseRequest : IMapFrom<Stage>, IMapTo<Stage>
	{
		public string? Name { get; set; }
		public string? Description { get; set; }
        public LearningMaterialStageRequest? LearningMaterial { get; set; }
    }
}
