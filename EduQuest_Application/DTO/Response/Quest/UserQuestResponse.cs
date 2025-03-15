using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response.Quests;

public class UserQuestResponse : IMapFrom<UserQuest>
{
    public string Id { get; set; }
    public string? Title { get; set; } //change to title?
    public int? Type { get; set; }// daily, one time, dev định nghĩa sẵn trong enum, cố định
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public int? PointToComplete { get; set; }
    public int? CurrentPoint { get; set; }
    public bool IsCompleted { get; set; }
    public bool? IsDaily { get; set; }

    public List<QuestRewardResponse> QuestRewards { get; set; } = new List<QuestRewardResponse>();
}
