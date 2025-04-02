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
    public string? Title { get; set; } //change name to title
    public int? Type { get; set; }// daily, one time, dev định nghĩa sẵn trong enum, cố định
    public int? QuestType { get; set; }// learning streak, complete courses,....
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public object[] QuestValue { get; set; } = Array.Empty<object>();
    public int PointToComplete { get; set; }
    public int? CurrentPoint { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsRewardClaimed { get; set; } = false;
    public object[] RewardType { get; set; } = Array.Empty<object>();
    public object[] RewardValue { get; set; } = Array.Empty<object>();
}
