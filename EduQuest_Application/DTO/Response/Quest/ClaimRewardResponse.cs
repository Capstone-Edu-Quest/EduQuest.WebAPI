using AutoMapper;
using EduQuest_Application.DTO.Response.Users;
using EduQuest_Application.DTO.Response.UserStatistics;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response.Quests;

public class ClaimRewardResponse : IMapFrom<User>
{
    public int? GoldAdded { get; set; } = 0;
    public int? ExpAdded { get; set; } = 0;
    public string? Coupon { get; set; }
    public int? BoosterAdded { get; set; } = 0;
    public UserStatisticDto statistic { get; set; }
    public List<string> mascotItem { get; set; }
    public List<string> equippedItems { get; set; }

    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<User, ClaimRewardResponse>()
            .ForMember(dest => dest.statistic, opt => opt.MapFrom(src => src.UserMeta))
            .ForMember(dest => dest.mascotItem, opt => opt.MapFrom(src => src.MascotItem
                .Select(s => s.ShopItemId)
                .ToList()))
            .ForMember(dest => dest.equippedItems, opt => opt.MapFrom(src => src.MascotItem
                .Where(m => m.IsEquipped)
                .Select(s => s.ShopItemId)
                .ToList()
            ));


    }
}
