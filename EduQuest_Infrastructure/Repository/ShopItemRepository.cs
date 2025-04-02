using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EduQuest_Infrastructure.Repository;

public class ShopItemRepository : GenericRepository<ShopItem>, IShopItemRepository
{
    private readonly ApplicationDbContext _context;

    public ShopItemRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ShopItem>> GetAllItemAsync()
    {
        return await _context.ShopItems.ToListAsync();
    }
    public async Task<ShopItem?> GetItemByName(string name)
    {
        return await _context.ShopItems.AsNoTracking().Where(a => a.Name.Equals(name)).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ShopItem?>> GetItemWithFilter(string name)
    {
        var query = _context.ShopItems.AsQueryable().AsNoTracking();
        if (!name.IsNullOrEmpty()) {
            query = query.Where(a => a.Name.Equals(name));
        }
        return await query.ToListAsync();
    }
}