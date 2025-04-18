using AutoMapper;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Courses;

public class AssignmentAttemptResponseForInstructor : IMapFrom<AssignmentAttempt>
{
    public string Id { get; set; }
    public string AssignmentId { get; set; }
    public string LessonId { get; set; }
    public string AnswerContent { get; set; }
    public double TotalTime { get; set; }
    public double AnswerScore { get; set; }
    public CommonUserResponse Author {  get; set; } = new CommonUserResponse();

    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<AssignmentAttempt, AssignmentAttemptResponseForInstructor>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User));
    }
}
