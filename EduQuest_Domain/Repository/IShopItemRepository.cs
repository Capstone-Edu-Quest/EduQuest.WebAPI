using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.PlatformStatisticDashBoard;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface IShopItemRepository : IGenericRepository<ShopItem>
{
    Task<IEnumerable<ShopItem>> GetAllItemAsync();
    Task<ShopItem?> GetItemByName(string name);
    Task<IEnumerable<ShopItem?>> GetItemWithFilter(string name);
    Task<ShopItemStatisticDto> GetShopItemStatisticsDto();
    Task UpdateShopItems(string Name, double price);
}
