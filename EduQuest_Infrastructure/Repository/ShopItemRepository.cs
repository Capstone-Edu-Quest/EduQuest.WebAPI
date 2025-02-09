using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

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
}