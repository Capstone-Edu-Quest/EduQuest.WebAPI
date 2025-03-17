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
    public int[] QuestValue { get; set; } = Array.Empty<int>();
    public int PointToComplete { get; set; }
    public int? CurrentPoint { get; set; }
    public bool IsCompleted { get; set; }
    public int[] RewardType { get; set; } = Array.Empty<int>();
    public int[] RewardValue { get; set; } = Array.Empty<int>();
}
