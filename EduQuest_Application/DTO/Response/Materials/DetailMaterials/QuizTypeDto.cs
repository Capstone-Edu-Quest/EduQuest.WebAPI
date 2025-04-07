using AutoMapper;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Materials.DetailMaterials;

public class QuizTypeDto : IMapFrom<Quiz>, IMapTo<Quiz>
{
    public int TimeLimit { get; set; }
    public decimal PassingPercentage { get; set; }
    public List<QuestionResponseDto> Questions { get; set; } = new List<QuestionResponseDto>();

    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<Quiz, QuizTypeDto>()
            .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));
            //.AfterMap((src, dest) =>
            //{
            //    dest.Questions ??= new List<QuestionResponseDto>
            //    {
            //        new QuestionResponseDto
            //        {
            //            QuestionTitle = "",
            //            MultipleAnswers = false,
            //            Answers = new List<AnswerResponseDto>
            //            {
            //                new AnswerResponseDto
            //                {
            //                    AnswerContent = "",
            //                    IsCorrect = true
            //                }
            //            }
            //        }
            //    };
            //});
    }
}

public class QuestionResponseDto : IMapFrom<Question>, IMapTo<Question>
{
    public string QuestionTitle { get; set; }
    public bool MultipleAnswers { get; set; }
    public List<AnswerResponseDto> Answers { get; set; } = new List<AnswerResponseDto>();

    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<Question, QuestionResponseDto>()
            .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers));
            //.AfterMap((src, dest) =>
            //{
            //    dest.Answers ??= new List<AnswerResponseDto>
            //    {
            //        new AnswerResponseDto
            //        {
            //            AnswerContent = "",
            //            IsCorrect = true
            //        }
            //    };
            //});
    }
}

public class AnswerResponseDto : IMapTo<Answer>, IMapFrom<Answer>
{
    public string AnswerContent { get; set; }
    public bool IsCorrect { get; set; }
}
