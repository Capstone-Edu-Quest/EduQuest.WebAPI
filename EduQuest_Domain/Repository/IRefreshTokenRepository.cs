using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
{
    Task<RefreshToken?> GetUserByIdAsync(string id);
    Task<RefreshToken?> GetTokenAsync(string refreshToken);
    Task<IEnumerable<RefreshToken?>> GetRefreshTokenByUserId(string id);
    Task RemoveRefreshTokenAsync(string id);

    Task<List<string>> GetValidTokensByUserIdAsync(string userId);
    Task DeleteTokensBulkAsync(List<string> tokens);
}
