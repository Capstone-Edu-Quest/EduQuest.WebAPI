using AutoMapper;
using EduQuest_Application.DTO.Response.Profiles.Heatmap;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Profiles;

public class LearnerProfileDto : IMapFrom<User>, IMapTo<User>
{
    public string? Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? RoleId { get; set; }
    public string? Phone { get; set; }
    public int? Level { get; set; } = 0;
    public string Status { get; set; } = null!;
    public string Headline { get; set; }
    public string Description { get; set; }
    public string AvatarUrl { get; set; }
    public bool isPro { get; set; }
    public LeanerProfileStatisticDto statistics { get; set; }
    public List<string> foxItems { get; set; }
    public List<string> equippedItems { get; set; }
    public int? TotalDays {  get; set; }
    public int? TotalMinutes {  get; set; }
    public List<LearningHeatmap> learningData { get; set; }
    public List<CompletedQuestDto> completedQuest { get; set; }
    public List<CourseProfileDto> RecentCourses { get; set; }

    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<User, LearnerProfileDto>()
            .ForMember(dest => dest.statistics, opt => opt.MapFrom(src => src.UserMeta))
            .ForMember(dest => dest.completedQuest, opt => opt.MapFrom(src => src.UserQuests.Where(a => a.IsCompleted)))
            .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.UserMeta.Level))
            .ForMember(dest => dest.equippedItems, opt => opt.MapFrom(src => src.MascotItem
                .Where(m => m.IsEquipped)
                .Select(s => s.ShopItemId)
                .ToList()
            ))
            .ForMember(dest => dest.foxItems, opt => opt.MapFrom(src => src.MascotItem
                  .Select(s => s.ShopItemId)
                  .ToList()
              ))
            .ForMember(dest => dest.isPro, opt => opt.MapFrom(src => src.Package != null && src.Package.ToLower() == "pro"));
    }
}
