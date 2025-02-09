using AutoMapper;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.UserStatistics;

public class UserStatisticDto : IMapFrom<UserStatistic>, IMapTo<UserStatistic>
{
    public string UserId { get; set; }
    public int? TotalActiveDay { get; set; }
    public int? MaxStudyStreakDay { get; set; }
    public DateTime? LastLearningDay { get; set; }
    public int? CompletedCourses { get; set; }
    public int? Gold { get; set; }
    public int? Exp { get; set; }
    public int? Level { get; set; }
    public int? StudyTime { get; set; }
    public int? TotalCourseCreated { get; set; }
    public int? TotalLearner { get; set; }
    public int? TotalReview { get; set; }

    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<UserStatistic, UserStatisticDto>()
            .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
    }
}
