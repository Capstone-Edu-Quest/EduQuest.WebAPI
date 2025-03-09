using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Extensions;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository;

public class QuestRepository : GenericRepository<Quest>, IQuestRepository
{
    private readonly ApplicationDbContext _context;

    public QuestRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Quest?> GetQuestById(string Id)
    {
        return await _context.Quests.Include(q => q.Rewards).FirstOrDefaultAsync(x => x.Id.Equals(Id));
    }

    public async Task<PagedList<Quest>> GetAllQuests(string? title, string? description, int? pointToComplete,
    int? type, int? timeToComplete, int page, int pageSize)
    {
        var result = _context.Quests.Include(q => q.Rewards).AsQueryable();

        if (!string.IsNullOrEmpty(title))
        {
            result = from r in result
                     where r.Title!.Contains(title)
                     select r;
        }
        if (!string.IsNullOrEmpty(description))
        {
            result = from r in result
                     where r.Description!.Contains(description)
                     select r;
        }
        if (pointToComplete.HasValue)
        {
            result = from r in result
                     where r.PointToComplete! >= pointToComplete.Value
                     select r;
        }
        if (type.HasValue)
        {
            result = from r in result
                     where r.Type! >= type.Value
                     select r;
        }
        if (timeToComplete.HasValue)
        {
            result = from r in result
                     where r.TimeToComplete! >= timeToComplete.Value
                     select r;
        }
        var response = await result.Pagination(page, pageSize).ToPagedListAsync(page, pageSize);
        return response;
    }
    

}
