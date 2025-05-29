using AutoMapper;
using EduQuest_Application.Mappings;

namespace EduQuest_Application.DTO.Response.Mascot;

public class UserMascotDto : IMapFrom<EduQuest_Domain.Entities.Mascot>, IMapTo<EduQuest_Domain.Entities.Mascot>
{ 
    public string ShopItemId { get; set; }
    public bool IsEquipped { get; set; }
    public int Gold {  get; set; }
    public Dictionary<string, int> ItemShards { get; set; } = new();

    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<EduQuest_Domain.Entities.Mascot, UserMascotDto>()
            .ForMember(dest => dest.ItemShards,
            opt => opt.MapFrom(src =>
                src.User.ItemShards
                   .GroupBy(i => i.Tags.Name)
                   .ToDictionary(
                       g => g.Key,
                       g => g.Sum(i => i.Quantity))
            ))
            .ForMember(dest => dest.Gold, opt => opt.MapFrom(src => src.User.UserMeta.Gold));

    }
}
