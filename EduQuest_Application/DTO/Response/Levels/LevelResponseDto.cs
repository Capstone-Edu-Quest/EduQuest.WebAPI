using AutoMapper;
using EduQuest_Application.DTO.Request.Level;
using EduQuest_Application.DTO.Response.Certificates;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;

namespace EduQuest_Application.DTO.Response.Levels;

public class LevelResponseDto : IMapFrom<Level>, IMapTo<Level>
{
    public int Id { get; set; }
    public int? Exp { get; set; }
    public object[] RewardType { get; set; } = Array.Empty<object>();
    public object[] RewardValue { get; set; } = Array.Empty<object>();

    /*public void MappingFrom(Profile profile)
    {
        profile.CreateMap<PagedList<Level>, PagedList<LevelResponseDto>>().ReverseMap();
    }*/
}
