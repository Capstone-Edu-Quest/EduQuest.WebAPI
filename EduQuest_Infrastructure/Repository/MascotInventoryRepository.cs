using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository;

public class MascotInventoryRepository : GenericRepository<Mascot>, IMascotInventoryRepository
{
    private readonly ApplicationDbContext _context;

    public MascotInventoryRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Mascot?> GetByUserIdAndItemIdAsync(string userId, string shopItemId)
    {
        return await _context.Mascots.AsNoTracking()
            .FirstOrDefaultAsync(i => i.UserId == userId && i.ShopItemId == shopItemId);
    }

    public async Task<IEnumerable<Mascot>> GetItemsByUserIdAsync(string userId)
    {
        return await _context.Mascots
            .Where(i => i.UserId == userId)
            .ToListAsync();
    }


    public async Task UpdateRangeMascot(List<string> items, string userId)
    {
        if (items == null || !items.Any())
            return; 

        await _context.Mascots
            .Where(m => m.UserId == userId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(m => m.IsEquipped, false));

        await _context.Mascots
            .Where(m => m.UserId == userId && items.Contains(m.ShopItemId))
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(m => m.IsEquipped, true));

    }

    //public async Task UnequipMascot()
    //{
    //    await 
    //}

    public async Task<IEnumerable<Mascot>> GetMascotByUserIdAndItemIdAsync(string userId, List<string> shopItemId)
    {
        return await _context.Mascots.AsNoTracking()
            .Where(i => i.UserId == userId && shopItemId.Contains(i.ShopItemId)).ToListAsync();
    }

    public async Task<IEnumerable<Mascot>> GetMascotEquippedByUserIdAsync(string userId)
    {
        return await _context.Mascots.AsNoTracking()
            .Where(i => i.UserId == userId && i.IsEquipped == true).ToListAsync();
    }
}