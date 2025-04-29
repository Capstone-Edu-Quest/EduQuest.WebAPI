using static EduQuest_Domain.Constants.Constants;
using System.ComponentModel.DataAnnotations;

namespace EduQuest_Application.DTO.Request.Level;

public class LevelDto
{
    public string? Id { get; set; }
    public int Level { get; set; }
    [Range(250, 250000, ErrorMessage ="exp ranged from 250 to 250000")]
    public int Exp { get; set; }
    public object[] RewardType { get; set; }
    public object[] RewardValue { get; set; }
}
