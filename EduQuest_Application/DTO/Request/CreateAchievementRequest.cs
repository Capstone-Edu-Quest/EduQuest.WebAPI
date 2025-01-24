using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request
{
	public class CreateAchievementRequest : IMapFrom<Achievement>, IMapTo<Achievement>
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public string? Color { get; set; }
		public List<string> ListBadgeId { get; set; }
    }
}
