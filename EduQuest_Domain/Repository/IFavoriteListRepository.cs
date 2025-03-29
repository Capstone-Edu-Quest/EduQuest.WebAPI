using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository
{
	public interface IFavoriteListRepository : IGenericRepository<FavoriteList>
	{
		Task<FavoriteList> GetFavoriteListByUserId(string userId);
        Task<bool> DeleteFavList(string userId, string courseId);
	}
}
