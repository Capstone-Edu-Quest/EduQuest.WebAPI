using AutoMapper;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Courses;

public class QuizAttemptsResponse : IMapFrom<QuizAttempt>
{
    public string Id { get; set; }
    public string QuizId { get; set; }
    public string LessonId { get; set; }
    public int CorrectAnswers { get; set; }
    public int IncorrectAnswers { get; set; }
    public double Percentage { get; set; }
    public decimal PassingPercentage { get; set; }
    public int AttemptNo { get; set; }
    public int TotalTime { get; set; }
    public DateTime? SubmitAt { get; set; }
    public bool IsPassed { get; set; } = false;
    public CommonUserResponse Author { get; set; } = new CommonUserResponse();
    public List<QuizAnswersResponse> Answers { get; set; } = new List<QuizAnswersResponse>();
    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<QuizAttempt, QuizAttemptsResponse>()
            .ForMember(dest => dest.PassingPercentage, opt => opt.MapFrom(src => src.Quiz.PassingPercentage))
            .ForMember(dest => dest.IsPassed, opt => opt.MapFrom(src => (double)src.Quiz.PassingPercentage < src.Percentage))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User));
    }
}
