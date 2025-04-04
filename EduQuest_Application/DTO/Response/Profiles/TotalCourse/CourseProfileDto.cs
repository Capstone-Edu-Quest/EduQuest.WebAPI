using AutoMapper;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Profiles.TotalCourse;

public class CourseProfileDto : IMapFrom<Course>, IMapTo<Course>
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? PhotoUrl { get; set; }
    public string Author { get; set; }

    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<Course, CourseProfileDto>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User.Username));
    }
}
