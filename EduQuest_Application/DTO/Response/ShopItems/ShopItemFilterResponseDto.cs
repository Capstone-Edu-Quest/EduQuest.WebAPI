using AutoMapper;
using EduQuest_Application.DTO.Response.Tags;
using EduQuest_Application.DTO.Response.Users;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;

namespace EduQuest_Application.DTO.Response.ShopItems;

public class ShopItemFilterResponseDto : IMapFrom<ShopItem>, IMapTo<ShopItem>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public ShopTagDto Tag { get; set; }

    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<Tag, ShopTagDto>();

        profile.CreateMap<ShopItem, ShopItemFilterResponseDto>()
            .ForMember(dest => dest.Tag, opt => opt.MapFrom(src => src.Tag));

    }
}

public class ShopTagDto
{
    public string TagId { get; set; }
    public string TagName { get; set; }
}
