
using AutoMapper;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;

namespace EduQuest_Application.DTO.Response.LearningPaths;

public class LearningPathDetailResponse : IMapFrom<LearningPath>
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int TotalTime { get; set; }
    public int TotalCourses { get; set; }
    public bool IsPublic { get; set; }
    public bool IsEnrolled { get; set; }
    public bool CreatedByExpert { get; set; }
    public CommonUserResponse CreatedBy { get; set; } = new CommonUserResponse();
    public List<LearningPathCourseResponse> Courses { get; set; } = new List<LearningPathCourseResponse>();
    public List<LearningPathTagResponse> Tags { get; set; } = new List<LearningPathTagResponse>();
    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<LearningPath, LearningPathDetailResponse>()
            .ForMember(dest => dest.TotalTime, opt => opt.MapFrom(src => src.TotalTimes)).ReverseMap();
        
    }
}
