using AutoMapper;
using EduQuest_Application.DTO.Response.Lessons;
using EduQuest_Application.Helper;
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
        public List<string>? RequirementList { get; set; } = new List<string>();
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

  //      public void Mapping(Profile profile)
  //      {
  //          profile.CreateMap<Course, CourseDetailResponse>()
		//		.ForMember(dest => dest.Requirement, opt => opt.MapFrom(src => ContentHelper.SplitString(src.Requirement, '.')));
		//}

    }

}
