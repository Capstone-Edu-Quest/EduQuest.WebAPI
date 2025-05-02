using AutoMapper;
using EduQuest_Application.DTO.Response.Tags;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.DTO.Response.Courses
{
    public class CourseSearchResponse : IMapFrom<Course>, IMapTo<Course>
    {
        public string Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? PhotoUrl { get; set; }
        public string Author { get; set; }
        public string CreatedBy { get; set; }
        public decimal Price { get; set; }
        public bool IsPublic { get; set; }
        public double? Rating { get; set; }
        public int TotalLesson { get; set; }
        public int TotalTime { get; set; }
        public int TotalReview { get; set; }
        public string ExpertId { get; set; }
        public string ExpertName { get; set; }
        public string? RejectedReason { get; set; }
        public double? ProgressPercentage { get; set; }
		public List<TagResponse>? ListTag { get; set; }

		public void MappingFrom(Profile profile)
        {
            profile.CreateMap<Course, CourseSearchResponse>()
            .ForMember(dest => dest.IsPublic, opt => opt.MapFrom(src => src.Status == StatusCourse.Public.ToString()))
            .ForMember(dest => dest.ExpertId, opt => opt.MapFrom(src => src.AssignTo))
            .ForMember(dest => dest.TotalLesson, opt => opt.MapFrom(src => src.CourseStatistic.TotalLesson))
            .ForMember(dest => dest.TotalTime, opt => opt.MapFrom(src => src.CourseStatistic.TotalTime))
            .ForMember(dest => dest.TotalReview, opt => opt.MapFrom(src => src.CourseStatistic.TotalReview))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User.Username))
			.ForMember(dest => dest.ListTag, opt => opt.MapFrom(src => src.Tags))
			.ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.CourseStatistic.Rating));
        }
    }
}
