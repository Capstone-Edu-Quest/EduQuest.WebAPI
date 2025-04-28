using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface IMascotInventoryRepository : IGenericRepository<Mascot>
{
    Task<IEnumerable<Mascot>> GetMascotEquippedByUserIdAsync(string userId);
    Task<Mascot?> GetByUserIdAndItemIdAsync(string userId, string shopItemId);
    Task<IEnumerable<Mascot>> GetItemsByUserIdAsync(string userId);
    Task UpdateRangeMascot(List<string> items, string userId);
    Task<IEnumerable<Mascot>> GetMascotByUserIdAndItemIdAsync(string userId, List<string> shopItemId);
}
