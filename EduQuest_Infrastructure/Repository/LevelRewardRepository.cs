using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository;

public class LevelRewardRepository : GenericRepository<LevelReward>, ILevelRewardRepository
{
    private readonly ApplicationDbContext _context;

    public LevelRewardRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<LevelReward>> GetByIdToList(string id)
    {
        return await _context.LevelRewards.Where(x => x.Id == id).ToListAsync();
    }

    public async Task<IEnumerable<LevelReward>> GetByLevelIdAsync(string levelId)
    {
        return await _context.LevelRewards.Where(x => x.LevelId == levelId).ToListAsync();
    }


    public void RemoveRangeAsync(IEnumerable<LevelReward> rewards)
    {
        if (rewards == null || !rewards.Any()) return;

        _context.LevelRewards.RemoveRange(rewards);
    }


}
