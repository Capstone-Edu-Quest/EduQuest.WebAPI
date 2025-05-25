using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response.Courses;

public class QuizAttemptResponse : IMapFrom<QuizAttempt>
{
    public int CorrectAnswers { get; set; }
    public int IncorrectAnswers { get; set; }
    public double Percentage { get; set; }
    public int AttemptNo { get; set; }
    public int TotalTime { get; set; }
    public DateTime? SubmitAt { get; set; }
    public bool isPassed {  get; set; } = false;
    public int? AddedItemShard { get; set; } = null;
    public Dictionary<string, int>? ItemShards { get; set; }
    public LevelUpNotiModel LevelInfo { get; set; } = new LevelUpNotiModel();
}
