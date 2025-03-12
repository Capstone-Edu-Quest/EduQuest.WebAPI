using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository
{
	public class FavoriteListRepository : GenericRepository<FavoriteList>, IFavoriteListRepository
	{
		private readonly ApplicationDbContext _context;

		public FavoriteListRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<bool> DeleteFavList(string userId, string courseId)
		{
			var favCourse = await _context.FavoriteLists.Where(x => x.UserId.Equals(userId)).FirstOrDefaultAsync();
			_context.FavoriteLists.Remove(favCourse!);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<List<FavoriteList>> GetFavoriteListByUserId(string userId)
		{
			return await _context.FavoriteLists.Include(x => x.User).Where(x => x.UserId == userId).ToListAsync();
		}
	}
}
