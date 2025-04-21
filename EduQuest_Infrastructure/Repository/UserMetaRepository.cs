using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.User;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository;

    public class UserMetaRepository : GenericRepository<UserMeta>, IUserMetaRepository
{
	private readonly ApplicationDbContext _context;

	public UserMetaRepository(ApplicationDbContext context) : base(context)
	{
		_context = context;
	}

	public async Task<UserMeta> GetByUserId(string userId)
	{
		return await _context.UserMetas.AsNoTracking().FirstOrDefaultAsync(x => x.UserId.Equals(userId));
	}
    public async Task<List<UserRanking>> GetLeaderboardData()
	{
        return await _context.UserMetas
        .Where(u => u.TotalStudyTime > 0)
        .Select(ranking => new UserRanking
        {
            UserId = ranking.UserId,
            TotalStudyTime = ranking.TotalStudyTime,
        })
        .ToListAsync();
    }
}
