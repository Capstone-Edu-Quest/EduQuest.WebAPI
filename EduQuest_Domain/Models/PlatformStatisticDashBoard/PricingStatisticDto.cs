namespace EduQuest_Domain.Models.PlatformStatisticDashBoard;

public class PricingStatisticDto
{
    public int? InstructorProSold { get; set; }
    public int? LearnerProSold { get; set; }
    public int? RenewRate { get; set; }
    public List<SubscriptionsCharDto> subscription { get; set; } = new List<SubscriptionsCharDto>
    {
        new SubscriptionsCharDto { Date = DateTime.Now.ToString("yyyy-MM-dd"), Count = 0 }
    };
}


public class SubscriptionsCharDto
{
    public string? Date { get; set; }
    public int? Count { get; set; }
}
