namespace EduQuest_Domain.Models.PlatformStatisticDashBoard;

public class ShopItemStatisticDto
{
    public int TotalItemSold { get; set; }
    public double AverageItemsPerUser { get; set; }
    public string? MostPurchasedItem { get; set; }
    public double TotalGoldFromSales { get; set; }
    public List<BestSaleItemDto> BestSaleItems { get; set; } = new List<BestSaleItemDto>
    {
        new BestSaleItemDto { name = "default-item", count = 0 }
    };
}


public class BestSaleItemDto
{
    public string? name { get; set; }
    public int count { get; set; }

}

