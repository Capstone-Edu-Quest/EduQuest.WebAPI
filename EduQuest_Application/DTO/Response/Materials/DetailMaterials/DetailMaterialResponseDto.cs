using AutoMapper;
using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Application.DTO.Response.UserStatistics;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;

namespace EduQuest_Application.DTO.Response.Materials.DetailMaterials;

public class DetailMaterialResponseDto : IMapFrom<Material>, IMapTo<Material>
{
    public string? Id { get; set; }
	public string? Type { get; set; }
	public string? Title { get; set; }
	public string? Description { get; set; }
	public double? Duration { get; set; }
	public VideoTypeDto? Video { get; set; }
	public string? Content { get; set; } //Document
	public QuizTypeDto? Quiz { get; set; }
	public AssignmentTypeDto? Assignment { get; set; }

	public void MappingFrom(Profile profile)
    {
		profile.CreateMap<Material, DetailMaterialResponseDto>()
/*		.ForMember(dest => dest.Quiz, opt => opt.MapFrom(src => src.Quiz))
		.ForMember(dest => dest.Assignment, opt => opt.MapFrom(src => src.Assignment))*/
		.ForMember(dest => dest.Video, opt => {
			opt.PreCondition(src => src.Type == GeneralEnums.TypeOfMaterial.Video.ToString()); // chỉ ánh xạ nếu là video
			opt.MapFrom(src => new VideoTypeDto
			{
				UrlMaterial = src.UrlMaterial,
				Thumbnail = src.Thumbnail,	
			});
		});
		//.ForPath(dest => dest.Video.Duration, opt => opt.MapFrom(src => src.Duration));

	}
}
