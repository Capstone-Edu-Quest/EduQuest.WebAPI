using AutoMapper;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.DTO.Response.Users;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;

namespace EduQuest_Application.DTO.Response.Certificates;

public class CertificateDto : IMapFrom<Certificate>, IMapTo<Certificate>
{
    public string Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserCerificateDto User { get; set; }
    public OverviewCourseResponse Course { get; set; }

    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<Certificate, CertificateDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.Users))
            .ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.Course));
    }
}

public class UserCerificateDto : IMapFrom<User>, IMapTo<User>
{
    public string? Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string Status { get; set; } = null!;
    public string Headline { get; set; }
    public string Description { get; set; }
    public string AvatarUrl { get; set; }
}
