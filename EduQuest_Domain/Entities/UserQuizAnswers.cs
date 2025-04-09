using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities;

public class UserQuizAnswers : BaseEntity
{
    public string QuestionId { get; set; }
    public string AnswerId { get; set; }
    public bool IsCorrect { get; set; }

    [JsonIgnore]
    public virtual Question Question { get; set; }
    [JsonIgnore]
    public virtual Answer Answer { get; set; }
}
