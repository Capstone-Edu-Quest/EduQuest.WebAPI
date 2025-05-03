using AutoMapper;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;

namespace EduQuest_Application.DTO.Response.LearningPaths;

public class MyLearningPathResponse : IMapFrom<LearningPath>
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int TotalTime { get; set; }
    public int TotalCourses {  get; set; }
    public bool IsPublic { get; set; }
    public bool IsEnrolled { get; set; }
    public DateTime? EnrollDate { get; set; }
    public bool CreatedByExpert { get; set; }
    public int TotalEnroller { get; set; } = 0;
    public List<LearningPathCoursePreview> LearningPathCourses { get; set; } = new List<LearningPathCoursePreview>();
    public CommonUserResponse CreatedBy { get; set; } = new CommonUserResponse();
    public List<LearningPathTagResponse> Tags { get; set; } = new List<LearningPathTagResponse>();
    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<LearningPath, MyLearningPathResponse>()
            .ForMember(dest => dest.TotalTime, opt => opt.MapFrom(src => src.TotalTimes)).ReverseMap();
        profile.CreateMap<PagedList<LearningPath>, PagedList<MyLearningPathResponse>>().ReverseMap();
    }
}
