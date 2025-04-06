using AutoMapper;
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
        public decimal? ProgressPercentage { get; set; }

        public void MappingFrom(Profile profile)
        {
            profile.CreateMap<Course, CourseSearchResponse>()
            .ForMember(dest => dest.IsPublic, opt => opt.MapFrom(src => src.Status == StatusCourse.Public.ToString() ? true : false));
        }
    }
}
