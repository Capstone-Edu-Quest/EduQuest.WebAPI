using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;

namespace EduQuest_Infrastructure.Repository;

public class LevelRewardRepository : GenericRepository<LevelReward>, ILevelRewardRepository
{
    private readonly ApplicationDbContext _context;

    public LevelRewardRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}
