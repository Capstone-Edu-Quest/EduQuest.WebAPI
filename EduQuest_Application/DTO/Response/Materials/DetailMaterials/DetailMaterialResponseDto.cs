using AutoMapper;
using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Application.DTO.Response.UserStatistics;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Materials.DetailMaterials;

public class DetailMaterialResponseDto : IMapFrom<Material>, IMapTo<Material>
{
    public string? Id { get; set; }
    public string? Type { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public VideoTypeDto? Videos { get; set; }
    public string? Content { get; set; } //Document
    public QuizTypeDto? Quizzes { get; set; }
    public AssignmentTypeDto? Assignments { get; set; }

    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<Material, DetailMaterialResponseDto>()
        .ForMember(dest => dest.Quizzes, opt => opt.MapFrom(src => src.Quiz))
        .ForMember(dest => dest.Assignments, opt => opt.MapFrom(src => src.Assignment));
        //.AfterMap((src, dest) =>
        //{
        //    dest.Videos ??= new VideoTypeDto
        //    {
        //        UrlMaterial = "",
        //        Duration = 0,
        //        Thumbnail = ""
        //    };

        //    dest.Quizzes ??= new QuizTypeDto
        //    {
        //        TimeLimit = 0,
        //        PassingPercentage = 0,
        //        Questions = new List<QuestionResponseDto>()
        //    };

        //    dest.Assignments ??= new AssignmentTypeDto
        //    {
        //        TimeLimit = 0,
        //        Question = "",
        //        AnswerLanguage = "",
        //        ExpectedAnswer = "",
                
        //    };
        //});


    }
}
