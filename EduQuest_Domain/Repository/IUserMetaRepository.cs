using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.User;
using EduQuest_Domain.Repository.Generic;


namespace EduQuest_Domain.Repository
{
	public interface IUserMetaRepository : IGenericRepository<UserMeta>
	{
		Task<UserMeta> GetByUserId(string userId);
		Task<List<UserRanking>> GetLeaderboardData();
	}
}
