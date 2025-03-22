using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .Include(a => a.Role)
            .FirstOrDefaultAsync(x => x.Email!.ToLower().Equals(email.ToLower()));
    }

    public async Task<List<User>?> GetByUserIds(List<string> ids)
    {

        var result = await _context.Users
            .Where(c => ids.Contains(c.Id))
            .ToListAsync();
        return result;
    }

	public async Task<User> GetUserById(string userId)
	{
        return await _context.Users.Include(x => x.Subscriptions).FirstOrDefaultAsync(x => x.Id == userId);
	}
}
