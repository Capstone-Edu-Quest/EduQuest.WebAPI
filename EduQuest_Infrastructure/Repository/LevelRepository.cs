using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Models.PlatformStatisticDashBoard;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository;

public class LevelRepository : GenericRepository<Levels>, ILevelRepository 
{
    private readonly ApplicationDbContext _context;

    public LevelRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PagedList<Levels>> GetLevelWithFiltersAsync(int? level, int? exp, int page, int eachPage)
    {
        var query = _context.Levels.AsQueryable().AsNoTracking();

        if (level.HasValue)
        {
            query = query.Where(l => l.Id == level.Value.ToString());
        }
        if (exp.HasValue)
        {
            query = query.Where(l => l.Exp == exp.Value);
        }

        int totalCount = await query.CountAsync();

        var items = await query.Skip(((int)page - 1) * (int)eachPage)
                               .Take((int)eachPage)
                               .ToListAsync();

        return new PagedList<Levels>(items, totalCount, (int)page, (int)eachPage);
    }

    public int GetExpByLevel(int level)
    {
        return _context.Levels.AsNoTracking()
            .Where(x => x.Level == level)
            .Select(x => x.Exp)
            .FirstOrDefault();
    }



    public async Task<IEnumerable<Levels>> GetByBatchLevelNumber(List<string> levelIds)
    {
        return await _context.Levels
            .Where(level => levelIds.Contains(level.Id))
            .ToListAsync();
    }
    public async Task<bool> IsLevelExist(int level)
    {
        return await _context.Levels.AnyAsync(l => l.Level == level);
    }
    public async Task ReArrangeLevelAfterDelete(int level)
    {
        int affectedRow = await _context.Levels
            .Where(l => l.Level > level)
            .ExecuteUpdateAsync(q =>
            q.SetProperty(l => l.Level, l => l.Level - 1));
    }


    public async Task<LevelExpStatisticDto> GetLevelExpStatistic()
    {
        var result = new LevelExpStatisticDto();

        var totalLevel = await _context.Users.AsNoTracking()
        .Where(u => u.Level != null)
        .SumAsync(u => u.Level!.Level);

        var totalExp = await _context.Users.AsNoTracking()
            .Where(u => u.Level != null)
            .SumAsync(u => u.Level!.Exp);

        //var avarageExp

        result.TotalEarnedExp = totalLevel;
        result.TotalEarnedLevel = totalExp;
        //result.AvarageExpPerDay = totalExp;
        //result.AverageLevel = totalLevel;
        var UserLevelChart = new UserLevelDto();

        var numberOfLevel = await _context.Users.AsNoTracking()
            .Where(u => u.Level != null)
            .GroupBy(u => u.Level.Level)
            .Select(a => new UserLevelDto
            {
                Level = a.Key,
                Count = a.Count()

            })
            .OrderBy(ul => ul.Level)
            .ToListAsync();

        result.UserLevels = numberOfLevel;
        return result;
    }
    public async Task<int> DeleteRangeByListId(List<string> ids)
    {
        if (ids == null || !ids.Any())
        {
            return 0;
        }

        var deletedCount = await _context.Levels
            .Where(l => ids.Contains(l.Id))
            .ExecuteDeleteAsync();

        return deletedCount;
    }
}
