using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response
{
	public class StageCourseResponse : IMapFrom<Stage>, IMapTo<Stage>
	{
        public string Id { get; set; }
        public int Level { get; set; }
		public string Name { get; set; }
		public int? TotalTime { get; set; }

	}
}
