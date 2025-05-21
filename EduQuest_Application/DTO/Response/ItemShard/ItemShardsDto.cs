using AutoMapper;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.ItemShard;

public class ItemShardsDto : IMapFrom<ItemShards>, IMapTo<ItemShards>
{
    public string TagName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<ItemShards, ItemShardsDto>()
            .ForMember(dest => dest.TagName, opt => opt.MapFrom(src => src.Tags != null ? src.Tags.Name : string.Empty))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

    }
}
