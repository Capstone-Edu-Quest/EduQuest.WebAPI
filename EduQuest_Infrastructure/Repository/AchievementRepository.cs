using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository
{
	public class AchievementRepository : GenericRepository<Achievement>, IAchievementRepository
	{
		private readonly ApplicationDbContext _context;

		public AchievementRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<Achievement> GetAchievementById(string Id)
		{
			return await _context.Achievements.Include(x => x.Users).Include(x => x.Badges).FirstOrDefaultAsync(x => x.Id.Equals(Id));
		}
	}
}
