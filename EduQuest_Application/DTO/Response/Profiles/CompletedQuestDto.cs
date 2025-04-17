using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Profiles;

public class CompletedQuestDto : IMapFrom<UserQuest>, IMapTo<UserQuest>
{
    public string Title { get; set; }
    public int? Type { get; set; }
    public int? QuestType { get; set; }
    public string? QuestValues { get; set; }
    public string startDate { get; set; }
    public string dueDate { get; set; }
    public bool isCompleted { get; set; }
    public int PointToComplete { get; set; }
    public int CurrentPoint { get; set; }
}
