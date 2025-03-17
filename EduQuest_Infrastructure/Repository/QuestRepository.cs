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
        return await _context.Quests.FirstOrDefaultAsync(x => x.Id.Equals(Id));
    }

    public async Task<PagedList<Quest>> GetAllQuests(string? title, int? questType, int? type, int? questValue,
        string userId, int page, int eachPage)
    {
        var result = _context.Quests.AsQueryable();

        if (!string.IsNullOrEmpty(title))
        {
            result = from r in result
                     where r.Title!.Contains(title)
                     select r;
        }
        if(type.HasValue)
        {
            result = from r in result
                     where r.Type! == type.Value
                     select r;
        }
        /*if (questValue.HasValue)
        {
            result = from r in result
                     where r.QuestValue! >= questValue.Value
                     select r;
        }*/
        if (questType.HasValue)
        {
            result = from r in result
                     where r.QuestType! == questType.Value
                     select r;
        }
        var response = await result.Pagination(page, eachPage).ToPagedListAsync(page, eachPage);
        return response;
    }
    

}
