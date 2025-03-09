using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Quests;

public class QuestResponse : IMapFrom<Quest>
{
    public string? Title { get; set; } //change name to title
    public bool? IsDaily { get; set; }
    public int? Type { get; set; }// daily, one time, dev định nghĩa sẵn trong enum, cố định
    public string? Description { get; set; }
    public int? PointToComplete { get; set; }
    public string? TimeToComplete { get; set; }

    public CommonUserResponse CreatedByUser { get; set; } = new CommonUserResponse();
    public List<QuestRewardResponse> QuestRewardResponses { get; set; } = new List<QuestRewardResponse>();
}
