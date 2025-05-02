using AutoMapper;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;

namespace EduQuest_Application.DTO.Response.Tags;

public class TagDto : IMapFrom<Tag>, IMapTo<Tag>
{
    public string Id { get; set; }
    public string Name { get; set; }
	//public string? Level { get; set; }
	//public int? Grade { get; set; }
	public string? Type { get; set; }
	public int Courses { get; set; }

    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<Tag, TagDto>()
            .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses.Count))
            .ReverseMap();
        profile.CreateMap<PagedList<Tag>, PagedList<TagDto>>().ReverseMap();
    }
}