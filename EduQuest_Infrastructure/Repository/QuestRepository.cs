using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository
{
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
	}
}
