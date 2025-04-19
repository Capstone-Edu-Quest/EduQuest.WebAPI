using AutoMapper;
using EduQuest_Application.Helper;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Profiles;

public class CompletedQuestDto : IMapFrom<UserQuest>, IMapTo<UserQuest>
{
    public string Title { get; set; }
    public int? Type { get; set; }
    public int? QuestType { get; set; }
    public object[] QuestValue { get; set; }
    public string startDate { get; set; }
    public string dueDate { get; set; }
    public bool isCompleted { get; set; }
    public string completedDate { get; set; }
    public int PointToComplete { get; set; }
    public int CurrentPoint { get; set; }
    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<UserQuest, CompletedQuestDto>()
            .ForMember(dest => dest.QuestValue, opt => opt.MapFrom(src => GeneralHelper.ToArray(src.QuestValues!)))
            .ForMember(dest => dest.completedDate, opt => opt.MapFrom(src => src.CompleteDate));
    }
}
