using AutoMapper;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.DTO.Response.Courses;

public class CourseExpertResponseDto : IMapFrom<Course>, IMapTo<Course>
{
    public string Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? PhotoUrl { get; set; }
    public string Author { get; set; }
    public string CreatedBy { get; set; }
    public decimal Price { get; set; }
    public double? Rating { get; set; }
    public int TotalLesson { get; set; }
    public int TotalTime { get; set; }
    public int TotalReview { get; set; }
    //public decimal? ProgressPercentage { get; set; }

    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<Course, CourseExpertResponseDto>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.TotalLesson, opt => opt.MapFrom(src => src.CourseStatistic.TotalLesson))
            .ForMember(dest => dest.TotalTime, opt => opt.MapFrom(src => src.CourseStatistic.TotalTime))
            .ForMember(dest => dest.TotalReview, opt => opt.MapFrom(src => src.CourseStatistic.TotalReview));
    }
}
