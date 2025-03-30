using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository;

public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;
    public RefreshTokenRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> GetUserByIdAsync(string id)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == id);
    }

    public async Task<IEnumerable<RefreshToken?>> GetRefreshTokenByUserId(string id)
    {
        return await _context.RefreshTokens.Where(x => x.UserId == id).ToListAsync();
    }

    public async Task<RefreshToken?> GetTokenAsync(string refreshToken)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
    }


    public async Task RemoveRefreshTokenAsync(string id)
    {
        var token = await _context.RefreshTokens.FindAsync(id);
        if (token != null)
        {
            _context.RefreshTokens.Remove(token);
        }
    }

    public async Task<List<RefreshToken>> GetValidTokensByUserIdAsync(string userId)
    {
        return await _context.RefreshTokens
            .AsNoTracking() 
            .Where(x => x.UserId == userId && x.ExpireAt > DateTime.UtcNow)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }


    public async Task DeleteTokensBulkAsync(List<string> tokens)
    {
        await _context.RefreshTokens
            .Where(rt => tokens.Contains(rt.Token))
            .ExecuteDeleteAsync();
    }
}
