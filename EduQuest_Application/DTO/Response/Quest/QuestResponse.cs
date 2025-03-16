using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Quests;

public class QuestResponse : IMapFrom<Quest>
{
    public string? Title { get; set; } //change name to title
    public int? Type { get; set; }// daily, one time, dev định nghĩa sẵn trong enum, cố định
    public int? QuestType { get; set; }// learning streak, complete courses,....
    public int? QuestValue { get; set; }

    public CommonUserResponse CreatedByUser { get; set; } = new CommonUserResponse();
    public List<QuestRewardResponse> QuestRewardResponses { get; set; } = new List<QuestRewardResponse>();
}
