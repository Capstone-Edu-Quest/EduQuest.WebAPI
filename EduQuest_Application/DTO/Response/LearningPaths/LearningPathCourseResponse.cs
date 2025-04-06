

using AutoMapper;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.LearningPaths;

public class LearningPathCourseResponse : IMapFrom<Course>
{
    public string Id { get; set; } 
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Color { get; set; }
    public decimal? Price { get; set; }
    public List<string>? RequirementList { get; set; }
    public string? Author { get; set; } 
    public string? CreatedBy { get; set; }
    public int Order { get; set; }
    public int TotalReview { get; set; } = 0;
    public double? Rating { get; set; } = 0;
    public int TotalLesson { get; set; } = 0;
    public int TotalTime { get; set; } = 0;
    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<Course, LearningPathCourseResponse>()
        .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.CourseStatistic.Rating))
        .ForMember(dest => dest.TotalLesson, opt => opt.MapFrom(src => src.CourseStatistic.TotalLesson))
        .ForMember(dest => dest.TotalTime, opt => opt.MapFrom(src => src.CourseStatistic.TotalTime))
        .ForMember(dest => dest.TotalReview, opt => opt.MapFrom(src => src.CourseStatistic.TotalReview));
    }
}
