using AutoMapper;
using EduQuest_Application.DTO.Response.Feedbacks;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.ShopItems;

public class ShopItemResponseDto : IMapFrom<ShopItem>, IMapTo<ShopItem>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string? TagId { get; set; }
    public string? TagName { get; set; }
    public bool IsOwned { get; set; }
    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<ShopItem, ShopItemResponseDto>()
            .ForMember(dest => dest.TagName, opt => opt.MapFrom(src => src.Tag.Name));
    }
}
