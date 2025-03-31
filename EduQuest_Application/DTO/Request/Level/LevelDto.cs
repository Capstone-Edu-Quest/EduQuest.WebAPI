using static EduQuest_Domain.Constants.Constants;
using System.ComponentModel.DataAnnotations;

namespace EduQuest_Application.DTO.Request.Level;

public class LevelDto
{
    public string? Id { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public object[] RewardType { get; set; }
    public object[] RewardValue { get; set; }
}
