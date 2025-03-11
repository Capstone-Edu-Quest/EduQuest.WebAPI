using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface IMascotInventoryRepository : IGenericRepository<Mascot>
{
    Task<Mascot?> GetByUserIdAndItemIdAsync(string userId, string shopItemId);
    Task<IEnumerable<Mascot>> GetItemsByUserIdAsync(string userId);
}
