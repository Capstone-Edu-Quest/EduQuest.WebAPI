using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request
{
	public class CreateAchievementRequest
	{
		public string Name { get; set; }
		public string Description { get; set; }
        public List<string> ListBadgeId { get; set; }
    }
}
