using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface IShopItemRepository : IGenericRepository<ShopItem>
{
    Task<IEnumerable<ShopItem>> GetAllItemAsync();
    Task<ShopItem?> GetItemByName(string name);
}
