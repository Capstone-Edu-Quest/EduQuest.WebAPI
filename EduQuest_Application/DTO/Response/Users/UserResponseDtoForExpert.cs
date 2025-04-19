using AutoMapper;
using EduQuest_Application.DTO.Response.Profiles;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Users;

public class UserResponseDtoForExpert : IMapFrom<User>, IMapTo<User>
{
    public string? Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string Status { get; set; } = null!;
    public string Headline { get; set; }
    public string Description { get; set; }
    public string AvatarUrl { get; set; }
    public string RoleId { get; set; }
    public string? AssignToExpertId { get; set; }
    public string? ExpertName { get; set; }
    public List<InstructorCertificateDto> InstructorCertificate { get; set; }


    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<User, UserResponseDtoForExpert>()
            .ForMember(dest => dest.InstructorCertificate, opt => opt.MapFrom(src => src.InstructorCertificates));
    }
}



