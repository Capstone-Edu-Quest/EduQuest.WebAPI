using AutoMapper;
using EduQuest_Application.DTO.Response.Boosters;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.UserStatistics;

public class UserStatisticDto : IMapFrom<UserMeta>, IMapTo<UserMeta>
{
    public string UserId { get; set; }
    public int? CurrentStreak { get; set; }
    public int? LongestStreak { get; set; }
    public DateTime? LastLearningDay { get; set; }
    public int? TotalCompletedCourses { get; set; }
    public int? Rank { get; set; } = 0;
    public BoosterResponseDto Booster { get; set; }
    public int? Gold { get; set; }
    public int? Exp { get; set; }
    public int? MaxExpLevel { get; set; }
    public int? Level { get; set; }
    public int? TotalStudyTime { get; set; }
    public int? TotalCourseCreated { get; set; }
    public int? TotalLearner { get; set; }
    public int? TotalReview { get; set; }
    public DateTime LastActive { get; set; }

    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<UserMeta, UserStatisticDto>()
            .ForMember(dest => dest.MaxExpLevel, opt => opt.MapFrom<MaxExpLevelResolver>())
            .ForMember(dest => dest.Booster, opt => opt.MapFrom(src =>
            src.User.Boosters
                .Where(b => b.DueDate >= DateTime.UtcNow) 
                .OrderByDescending(b => b.BoostValue)
                .Select(b => new BoosterResponseDto
                {
                    BoostExp = b.BoostValue,
                    BoostGold = b.BoostValue 
                })
                .FirstOrDefault() ?? new BoosterResponseDto { BoostExp = 0.0, BoostGold = 0.0 } 
        ))
            .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            
    }
}
