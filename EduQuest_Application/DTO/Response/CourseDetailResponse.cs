using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response
{
	public class CourseDetailResponse : IMapFrom<Course>, IMapTo<Course>
	{
		public string Title { get; set; } = string.Empty;
		public string? Description { get; set; }
		public string? PhotoUrl { get; set; }
        public int NumOfPurchased { get; set; }
		public DateTime? LastUpdated { get; set; }
		public decimal Price { get; set; }
		public decimal? DiscountPrice { get; set; }
		public double? Rating { get; set; }
		public AuthorCourseResponse Author { get; set; }
        public List<StageCourseResponse> ListStage { get; set; }

    }
}
