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

    
}