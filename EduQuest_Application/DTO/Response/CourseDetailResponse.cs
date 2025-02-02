using AutoMapper;
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
		public string? Requirement { get; set; }
		public string? Feature { get; set; }
		public DateTime? LastUpdated { get; set; }
		public decimal Price { get; set; }	
		public decimal? DiscountPrice { get; set; }
		public AuthorCourseResponse? Author { get; set; }
        public List<StageCourseResponse>? ListStage { get; set; }
		public List<TagResponse>? ListTag { get; set; }
		public int? TotalLearner { get; set; }
		public double? Rating { get; set; }
		public int? TotalReview { get; set; }

		//public void Mapping(Profile profile)
		//{
		//	profile.CreateMap<Course, CourseDetailResponse>()
		//		.ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User)) // Map Author
		//		.ForMember(dest => dest.ListStage, opt => opt.MapFrom(src => src.Stages)); // Map ListStage
		//}

	}
}
