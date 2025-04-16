using AutoMapper;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Courses;

public class QuizAnswersResponse : IMapFrom<UserQuizAnswers>
{
    public string Id { get; set; }
    public string QuestionId { get; set; }
    public string Question { get; set; }
    public string AnswerId { get; set; }
    public string Answer { get; set; }
    public bool IsCorrect { get; set; }
    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<UserQuizAnswers, QuizAnswersResponse>()
            .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question.QuestionTitle))
            .ForMember(dest => dest.Answer, opt => opt.MapFrom(src => src.Answer.AnswerContent));
    }
}
