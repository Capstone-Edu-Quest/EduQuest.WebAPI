using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response.Materials
{
	public class ContentInLessonResponse 
	{
        public string Id { get; set; }
        public string Type { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public double? Duration { get; set; }
		public string? Status { get; set; }
	}
}
