using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Extensions;
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

		public async Task<FavoriteList> GetFavoriteListByUserId(string userId)
		{
			return await _context.FavoriteLists
				.Include(x => x.User).Include(x => x.Courses)
				.FirstOrDefaultAsync(x => x.UserId == userId);
		}
		public async Task<PagedList<FavoriteList>> GetFavoriteListByUserId(string userId, string? title, string? description,
        decimal? price, string? requirement, string? feature, int page, int eachPage)
		{
			var result = _context.FavoriteLists.Include(f => f.Courses)
				.Include(f => f.User)
				.Where(f => f.UserId == userId).AsQueryable();
            #region Filter
            if (!string.IsNullOrEmpty(title))
			{
                result = result.Where(f => f.Courses!.Any(c => c.Title.Contains(title)));
            }
			if (!string.IsNullOrEmpty(description))
			{
				result = result.Where(f => f.Courses!.Any(c => c.Description.Contains(description)));
			}
			if (price.HasValue)
			{
				result = result.Where(result => result.Courses!.Any(c => c.Price >= price));
			}
			if(!string.IsNullOrEmpty(requirement))
			{
                result = result.Where(f => f.Courses!.Any(c => c.Requirement.Contains(requirement)));
            }
            if (!string.IsNullOrEmpty(feature))
            {
                result = result.Where(f => f.Courses!.Any(c => c.Feature.Contains(feature)));
            }
            #endregion
            return await result.Pagination(page, eachPage).ToPagedListAsync(page, eachPage);
        }

    }
}
