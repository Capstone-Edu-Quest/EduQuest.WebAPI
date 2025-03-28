using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository;

public class LevelRepository : GenericRepository<Level>, ILevelRepository 
{
    private readonly ApplicationDbContext _context;

    public LevelRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PagedList<Level>> GetLevelWithFiltersAsync(int? level, int? exp, int page, int eachPage)
    {
        var query = _context.Levels.AsQueryable();

        if (level.HasValue)
        {
            query = query.Where(l => l.LevelNumber == level.Value);
        }
        if (exp.HasValue)
        {
            query = query.Where(l => l.Exp == exp.Value);
        }

        int totalCount = await query.CountAsync();

        var items = await query.Skip(((int)page - 1) * (int)eachPage)
                               .Take((int)eachPage)
                               .ToListAsync();

        return new PagedList<Level>(items, totalCount, (int)page, (int)eachPage);
    }

}
