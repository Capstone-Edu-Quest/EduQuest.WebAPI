using AutoMapper;
using EduQuest_Application.DTO.Response.Mascot;
using EduQuest_Application.DTO.Response.UserStatistics;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;

namespace EduQuest_Application.DTO.Response;

public class UserResponseDto : IMapFrom<User>, IMapTo<User>
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
    public UserStatisticDto statistic { get; set; }
    public List<string> mascotItem { get; set; }
    public List<string> equippedItems { get; set; }

    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<User, UserResponseDto>()
            .ForMember(dest => dest.statistic, opt => opt.MapFrom(src => src.UserMeta))
            .ForMember(dest => dest.mascotItem, opt => opt.MapFrom(src => src.MascotItem
                .Select(s => s.ShopItemId)
                .ToList()))
            .ForMember(dest => dest.equippedItems, opt => opt.MapFrom(src => src.MascotItem
                .Where(m => m.IsEquipped)
                .Select(s => s.ShopItemId)
                .ToList()
            ));
            

        profile.CreateMap<PagedList<User>, PagedList<UserResponseDto>>().ReverseMap();
    }
}
