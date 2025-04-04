using EduQuest_Application.DTO.Response.Lessons;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.CourseStatistics;

namespace EduQuest_Application.DTO.Response.Courses
{
	public class CourseDetailResponseForIntructor : IMapFrom<Course>, IMapTo<Course>
	{
		public string Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string? Description { get; set; }
		public string? PhotoUrl { get; set; }
		public string? Status { get; set; }
		public List<string>? RequirementList { get; set; } = new List<string>();
		public DateTime? LastUpdated { get; set; }
		public decimal Price { get; set; }
		public List<LessonCourseResponse>? ListLesson { get; set; }
		public List<TagResponse>? ListTag { get; set; }
		public int? TotalLearner { get; set; }
		public double? Rating { get; set; }
		public int? TotalReview { get; set; }
		public int? TotalTime { get; set; }
		public int? TotalLesson { get; set; }
		public int? TotalInCart { get; set; }
		public int? TotalInWishList { get; set; }
        public List<ChartInfo> CourseEnrollOverTime { get; set; }
        public List<ChartInfo> CourseRatingOverTime { get; set; }
    }
}
