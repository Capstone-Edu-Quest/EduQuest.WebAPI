using AutoMapper;
using EduQuest_Application.DTO.Request.Level;
using EduQuest_Application.DTO.Response.Certificates;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;

namespace EduQuest_Application.DTO.Response.Levels;

public class LevelResponseDto : IMapFrom<Level>, IMapTo<Level>
{
    public int? LevelNumber { get; set; }
    public int? Exp { get; set; }
    public IEnumerable<LevelsRewardResponseDto> Rewards { get; set; }

    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<Level, LevelResponseDto>()
            .ForMember(dest => dest.Rewards, opt => opt.MapFrom(src => src.LevelRewards))
            .ReverseMap();
        profile.CreateMap<PagedList<Level>, PagedList<LevelResponseDto>>().ReverseMap();
    }
}
