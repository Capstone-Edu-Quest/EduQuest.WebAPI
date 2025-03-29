using EduQuest_Application.DTO.Response.Lessons;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Courses
{
	public class CourseDetailResponse : IMapFrom<Course>, IMapTo<Course>
    {
        public string Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Requirement { get; set; }
        public string? Feature { get; set; }
        public DateTime? LastUpdated { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public AuthorCourseResponse? Author { get; set; }
        public List<LessonCourseResponse>? ListLesson { get; set; }
        public List<TagResponse>? ListTag { get; set; }
        public int? TotalLearner { get; set; }
        public double? Rating { get; set; }
        public int? TotalReview { get; set; }
        public decimal? Progress { get; set; }

        //public void Mapping(Profile profile)
        //{
        //	profile.CreateMap<Course, CourseDetailResponse>()
        //		.ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User)) // Map Author
        //		.ForMember(dest => dest.ListStage, opt => opt.MapFrom(src => src.Stages)); // Map ListStage
        //}

    }
}
