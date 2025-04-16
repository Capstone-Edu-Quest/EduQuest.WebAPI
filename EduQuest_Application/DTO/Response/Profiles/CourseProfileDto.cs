using AutoMapper;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.DTO.Response.Lessons;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Profiles;

public class CourseProfileDto : IMapFrom<Course>, IMapTo<Course>
{
    public string Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? PhotoUrl { get; set; }
    public decimal? Price { get; set; }
    public List<string>? RequirementList { get; set; }
    public string Author { get; set; }
    public string? Status { get; set; }
    public string CreatedBy { get; set; }
    public double? Rating { get; set; }
    public int? TotalLesson { get; set; }
    public int? TotalTime { get; set; }
    public int? TotalReview { get; set; }

    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<Course, CourseProfileDto>()
        .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.CourseStatistic.Rating))
        .ForMember(dest => dest.TotalLesson, opt => opt.MapFrom(src => src.CourseStatistic.TotalLesson))
        .ForMember(dest => dest.TotalTime, opt => opt.MapFrom(src => src.CourseStatistic.TotalTime))
        .ForMember(dest => dest.TotalReview, opt => opt.MapFrom(src => src.CourseStatistic.TotalReview))
        .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User.Username));

        //profile.CreateMap<PagedList<Course>, PagedList<FavoriteCourseResponse>>().ReverseMap();
    }
}
