using AutoMapper;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response.Materials.DetailMaterials;

public class LearnerMaterialResponseDto : IMapFrom<Material>, IMapTo<Material>
{
    public string? Id { get; set; }
    public string Type { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int? Version { get; set; }
    public double? Duration { get; set; }
    public string Status { get; set; }
    //public string LessonId { get; set; }

    public VideoTypeDto? Video { get; set; }
    public string? Content { get; set; } //Document
    public QuizTypeDto? Quiz { get; set; }
    public AssignmentTypeDto? Assignment { get; set; }

    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<Material, LearnerMaterialResponseDto>()
        .ForMember(dest => dest.Quiz, opt => opt.MapFrom(src => src.Quiz))
        .ForMember(dest => dest.Assignment, opt => opt.MapFrom(src => src.Assignment))
        .ForPath(dest => dest.Video.UrlMaterial, opt => opt.MapFrom(src => src.UrlMaterial));
        //.ForPath(dest => dest.Video.Duration, opt => opt.MapFrom(src => src.Duration));

    }
}
