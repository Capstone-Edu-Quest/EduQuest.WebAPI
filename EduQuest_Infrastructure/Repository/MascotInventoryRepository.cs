using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository;

public class MascotInventoryRepository : GenericRepository<MascotInventory>, IMascotInventoryRepository
{
    private readonly ApplicationDbContext _context;

    public MascotInventoryRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<MascotInventory?> GetByUserIdAndItemIdAsync(string userId, string shopItemId)
    {
        return await _context.MascotItems
            .FirstOrDefaultAsync(i => i.UserId == userId && i.ShopItemId == shopItemId);
    }

    public async Task<IEnumerable<MascotInventory>> GetItemsByUserIdAsync(string userId)
    {
        return await _context.MascotItems
            .Where(i => i.UserId == userId)
            .ToListAsync();
    }

    
}