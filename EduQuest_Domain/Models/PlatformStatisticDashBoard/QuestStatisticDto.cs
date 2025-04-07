namespace EduQuest_Domain.Models.PlatformStatisticDashBoard;

public class QuestStatisticDto
{
    public int TotalCreatedQuests { get; set; }
    public int TotalCompletedQuests { get; set; }
    public double AverageCompletedQuestsPerUser { get; set; }
    public List<QuestCompletionDto> QuestCompletion { get; set; }
}

public class QuestCompletionDto
{
    public string Date { get; set; }
    public int Count { get; set; }

}

