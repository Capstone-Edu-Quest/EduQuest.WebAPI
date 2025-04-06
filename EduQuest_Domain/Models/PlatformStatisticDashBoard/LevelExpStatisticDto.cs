namespace EduQuest_Domain.Models.PlatformStatisticDashBoard;

public class LevelExpStatisticDto
{
    public int TotalEarnedExp { get; set; }
    public int AvarageExpPerDay { get; set; }

    public int TotalEarnedLevel { get; set; }
    public int AverageLevel { get; set; }
    public List<UserLevelDto> UserLevels { get; set; } = new List<UserLevelDto>
    {
        new UserLevelDto { Level = 0, Count = 0 } 
    };

}

public class UserLevelDto
{
    public int Level { get; set; }
    public int Count { get; set; }
}
