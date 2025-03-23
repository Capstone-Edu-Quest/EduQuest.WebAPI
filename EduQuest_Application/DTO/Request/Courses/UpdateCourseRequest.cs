using EduQuest_Application.DTO.Request.Lessons;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Request.Courses
{
	public class UpdateCourseRequest : IMapFrom<Course>, IMapTo<Course>
	{
        public string CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
		public string? Description { get; set; }
		public string? PhotoUrl { get; set; }
		public string? Requirement { get; set; }
		public string? Feature { get; set; }
		public decimal? Price { get; set; }
		public bool? IsRequired { get; set; }

		public List<CreateLessonRequest>? LessonCourse { get; set; }
	}
}
