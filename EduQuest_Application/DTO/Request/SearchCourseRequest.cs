using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request
{
	public class SearchCourseRequest
	{
        public string? KeywordName { get; set; }
        public DateTime? DateTo { get; set; }
		public DateTime? DateFrom { get; set; }
        public List<Tag?>? TagList { get; set; }
        public string? Author { get; set; }
        public int? Rating { get; set; }

    }
}
