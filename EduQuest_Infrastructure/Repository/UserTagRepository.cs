using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository;

public class UserTagRepository : GenericRepository<UserTag>, IUserTagRepository
{
    private readonly ApplicationDbContext _context;
    public UserTagRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task DeleteByUserIdAsync(string userId)
    {
        var userTags = await _context.UserTags
            .Where(ut => ut.UserId == userId)
            .ToListAsync();

        if (userTags.Any())
        {
            _context.UserTags.RemoveRange(userTags);
        }
    }

    public async Task BulkCreateAsync(List<UserTag> userTags)
    {
        if (userTags.Any())
        {
            await _context.UserTags.AddRangeAsync(userTags);
        }
    }
}
