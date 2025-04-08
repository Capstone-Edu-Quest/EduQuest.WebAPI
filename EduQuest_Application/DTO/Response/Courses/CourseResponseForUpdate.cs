using AutoMapper;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.DTO.Response.Courses
{
	public class CourseResponseForUpdate : IMapFrom<Course>, IMapTo<Course>
	{
		public string Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string? Description { get; set; }
		public string? PhotoUrl { get; set; }
		public List<string>? RequirementList { get; set; } = new List<string>();
		public DateTime? LastUpdated { get; set; }
		public decimal Price { get; set; }
		public bool IsPublic { get; set; }
		public List<Lesson>? Lessons { get; set; }

		public void Mapping(Profile profile)
		{
			profile.CreateMap<Course, CourseDetailResponse>()
				.ForMember(dest => dest.IsPublic, opt => opt.MapFrom(src => src.Status == StatusCourse.Public.ToString()));
		}
	}
}
