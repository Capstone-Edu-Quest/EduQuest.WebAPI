using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request.UserMetas
{
	public class UpdateUserProgressRequest
	{
		public string MaterialId { get; set; }
		public string LessonId { get; set; }
		public double? Time { get; set; }
	}
}
