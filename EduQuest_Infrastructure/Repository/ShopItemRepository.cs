using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.PlatformStatisticDashBoard;
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

    public async Task<IEnumerable<ShopItem>> GetByNamesAsync(List<string> names)
    {
        return await _context.ShopItems
                             .AsNoTracking()
                             .Where(item => names.Contains(item.Name))
                             .ToListAsync();
    }

    public async Task<IEnumerable<ShopItem>> GetAllItemAsync()
    {
        return await _context.ShopItems.AsNoTracking().ToListAsync();
    }
    public async Task<ShopItem?> GetItemByName(string name)
    {
        return await _context.ShopItems.AsNoTracking().Where(a => a.Name.Equals(name)).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ShopItem?>> GetItemWithFilter(string name, bool isGold)
    {
        var query = _context.ShopItems.AsQueryable().AsNoTracking();
        if (!name.IsNullOrEmpty()) {
            query = query.Where(a => a.Name.Equals(name));
        }
        if (!isGold)
        {
            query = query.Where(a => a.TagId == null);
        }
        return await query.ToListAsync();
    }

    public async Task<ShopItemStatisticDto> GetShopItemStatisticsDto()
    {
        var result = new ShopItemStatisticDto();

        var totalItems = await _context.Mascots.CountAsync();

        var totalUsers = await _context.Mascots
            .Select(x => x.UserId)
            .Distinct()
            .CountAsync();

        var averageItemsPerUser = totalUsers == 0 ? 0 : (double)totalItems / totalUsers;
        result.AverageItemsPerUser = averageItemsPerUser;

        var totalItemSold = await _context.Mascots
                            .AsNoTracking()
                            .CountAsync(a => a.UserId != null);

        var mostPurchasedItem = await _context.Mascots
                            .AsNoTracking()
                            .GroupBy(m => m.ShopItemId)
                            .Select(g => new { ShopItemId = g.Key, Count = g.Count() })
                            .OrderByDescending(x => x.Count)
                            .FirstOrDefaultAsync();

        result.MostPurchasedItem = mostPurchasedItem?.ShopItemId;

        var bestSaleItems = await _context.Mascots
                            .AsNoTracking()
                            .GroupBy(m => m.ShopItemId)
                            .Select(g => new BestSaleItemDto
                            {
                                name = g.Key,
                                count = g.Count()
                            })
                            .ToListAsync();

        var totalGold = await (from mascot in _context.Mascots
                               join item in _context.ShopItems
                               on mascot.ShopItemId equals item.Name
                               select item.Price)
                              .SumAsync();

        result.BestSaleItems = bestSaleItems;
        result.TotalGoldFromSales = totalGold;
        result.TotalItemSold = totalItemSold;

        return result;
    }



    public async Task UpdateShopItems(string Name, double price)
    {
        await _context.ShopItems
            .Where(a => a.Name.ToLower().Equals(Name.ToLower()))
            .ExecuteUpdateAsync(set => set.SetProperty(a => a.Price, price));
    }

    public async Task DeleteAllAsync()
    {
        await _context.ShopItems
            .ExecuteDeleteAsync(); 
    }

}