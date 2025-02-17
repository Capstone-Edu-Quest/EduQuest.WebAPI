using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository
{
    public interface IBadgeRepository : IGenericRepository<Badge>
	{
        IQueryable<Badge> GetBadgesWithFilters(string name, string description, string iconUrl, string color);

    }
}
