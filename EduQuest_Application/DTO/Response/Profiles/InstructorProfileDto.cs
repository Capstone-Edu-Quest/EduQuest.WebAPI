using AutoMapper;
using EduQuest_Application.DTO.Response.Users;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;

namespace EduQuest_Application.DTO.Response.Profiles;

public class InstructorProfileDto : IMapFrom<User>, IMapTo<User>
{
    public string? Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? RoleId { get; set; }
    public string? Phone { get; set; }
    public string? Status { get; set; }
    public string Headline { get; set; }
    public string Description { get; set; }
    public string AvatarUrl { get; set; }
    public bool isPro { get; set; }
    public int? TotalLearners { get; set; } = 0;
    public int? TotalReviews { get; set; } = 0;
    public int? AvarageReviews { get; set; } = 0;
    public string? AssignToExpertId { get; set; }
    public string? ExpertName { get; set; }

    public List<InstructorCertificateDto> InstructorCertificate { get; set; }
    public List<CourseProfileDto> Courses { get; set; }

    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<User, InstructorProfileDto>()
            .ForMember(dest => dest.InstructorCertificate, opt => opt.MapFrom(src => src.InstructorCertificates))
            .ForMember(dest => dest.isPro, opt => opt.MapFrom(src => src.Package != null && src.Package.ToLower() == "pro"));
    }

}


public class InstructorCertificateDto : IMapFrom<InstructorCertificate>, IMapTo<InstructorCertificate>
{
    public string? Id { get; set; }
    public string? CertificateUrl { get; set; }

}
