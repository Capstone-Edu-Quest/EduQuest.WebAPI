namespace EduQuest_Application.DTO.Request.Level;

public class LevelDto
{
    public int Level { get; set; }
    public int Exp { get; set; }
    public List<LevelRewardDto> Rewards { get; set; } = new List<LevelRewardDto>();
}
