

using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository;

public class ItemShardRepository : GenericRepository<ItemShards>, IItemShardRepository
{
    private readonly ApplicationDbContext _context;
    public ItemShardRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<ItemShards?> GetItemShardsByTagId(string tagId, string userId)
    {
        return await _context.ItemShards.Where(i => i.TagId == tagId && i.UserId == userId).FirstOrDefaultAsync();
    }
}
