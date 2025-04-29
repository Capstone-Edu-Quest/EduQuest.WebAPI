using AutoMapper;
using EduQuest_Application.DTO.Response.Certificates;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.DTO.Response.Courses
{
    public class OverviewCourseResponse : IMapFrom<Course>, IMapTo<Course>
    {
        public string Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? PhotoUrl { get; set; }
        public decimal? Price { get; set; }
        public bool IsPublic { get; set; }
        public List<string>? RequirementList { get; set; }
		public string Author { get; set; }
		public string? Status { get; set; }
        public string CreatedBy { get; set; }
        public double? Rating { get; set; }
        public int? TotalLesson { get; set; }
        public double? TotalTime { get; set; }
        public int? TotalReview { get; set; }
		public int? TotalLearner { get; set; }
		public List<TagResponse>? ListTag { get; set; }

		public void MappingFrom(Profile profile)
        {
            profile.CreateMap<Course, OverviewCourseResponse>()
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.CourseStatistic.Rating))
            .ForMember(dest => dest.IsPublic, opt => opt.MapFrom(src => src.Status == StatusCourse.Public.ToString()))
            .ForMember(dest => dest.TotalLesson, opt => opt.MapFrom(src => src.CourseStatistic.TotalLesson))
            .ForMember(dest => dest.TotalTime, opt => opt.MapFrom(src => src.CourseStatistic.TotalTime))
			.ForMember(dest => dest.TotalLearner, opt => opt.MapFrom(src => src.CourseStatistic.TotalLearner))
			.ForMember(dest => dest.ListTag, opt => opt.MapFrom(src => src.Tags))
			.ForMember(dest => dest.TotalReview, opt => opt.MapFrom(src => src.CourseStatistic.TotalReview));
            //profile.CreateMap<PagedList<Course>, PagedList<FavoriteCourseResponse>>().ReverseMap();
        }
    }

}
