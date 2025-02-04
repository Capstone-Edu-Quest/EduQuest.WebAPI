using AutoMapper;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response;

public class UserResponseDto : IMapFrom<User>, IMapTo<User>
{
    public string? Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string Status { get; set; } = null!;
    public string AvatarUrl { get; set; }
    public string RoleId { get; set; }

    //public void MappingFrom(Profile profile)
    //{
    //    profile.CreateMap<User, UserResponseDto>()
    //     .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Username))   
    //     .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))          
    //     .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.AvatarUrl));
    //}
}
