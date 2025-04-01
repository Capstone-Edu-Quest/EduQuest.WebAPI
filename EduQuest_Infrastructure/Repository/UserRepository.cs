using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using static EduQuest_Domain.Enums.GeneralEnums;

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
            .AsNoTracking()
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
        return await _context.Users.Include(x => x.Subscription).FirstOrDefaultAsync(x => x.Id == userId);
	}

	public async Task<bool> UpdateUserPackageAccountType(string userId)
	{
        int affectedRow = await _context.Users.Where(u => u.Id == userId).ExecuteUpdateAsync(u => u.SetProperty(u => u.Package, PackageEnum.Free.ToString()));
		return affectedRow > 0;
	}
}
