using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface IMascotInventoryRepository : IGenericRepository<MascotInventory>
{
    Task<MascotInventory?> GetByUserIdAndItemIdAsync(string userId, string shopItemId);
    Task<IEnumerable<MascotInventory>> GetItemsByUserIdAsync(string userId);
}
