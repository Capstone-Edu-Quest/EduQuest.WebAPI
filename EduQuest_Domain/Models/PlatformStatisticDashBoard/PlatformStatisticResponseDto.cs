using EduQuest_Domain.Models.Coupons;

namespace EduQuest_Domain.Models.PlatformStatisticDashBoard;

public class PlatformStatisticResponseDto
{
    public LevelExpStatisticDto LevelExp { get; set; } = new LevelExpStatisticDto();
    public QuestStatisticDto Quests { get; set; } = new QuestStatisticDto();
    public ShopItemStatisticDto ShopItems { get; set; } = new ShopItemStatisticDto();
    public PricingStatisticDto Pricing { get; set; } = new PricingStatisticDto();
    public CouponStatistics Coupons { get; set; } = new CouponStatistics();
}
