using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;

namespace EduQuest_Infrastructure.Repository;

public class BadgeRepository : GenericRepository<Badge>, IBadgeRepository
{
	private readonly ApplicationDbContext _context;

	public BadgeRepository(ApplicationDbContext context) : base(context)
	{
		_context = context;
	}

    public IQueryable<Badge> GetBadgesWithFilters(string name, string description, string iconUrl, string color)
    {
        var query = _context.Set<Badge>().AsQueryable();

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(b => b.Name.Contains(name));
        }
        if (!string.IsNullOrEmpty(description))
        {
            query = query.Where(b => b.Description.Contains(description));
        }
        if (!string.IsNullOrEmpty(iconUrl))
        {
            query = query.Where(b => b.IconUrl.Contains(iconUrl));
        }
        if (!string.IsNullOrEmpty(color))
        {
            query = query.Where(b => b.Color.Contains(color));
        }

        return query;
    }

}
