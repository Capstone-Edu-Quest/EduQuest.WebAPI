using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities;

public class UserQuest
{
    public string? Title { get; set; } //change to title?
    public int? Type { get; set; }// daily, one time, dev định nghĩa sẵn trong enum, cố định
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public int? PointToComplete { get; set; }
    public int? CurrentPoint {  get; set; }
    public bool IsCompleted { get; set; }

    [JsonIgnore]
    public virtual ICollection<QuestReward> Rewards { get; set; }
}
