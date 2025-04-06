using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Models.PlatformStatisticDashBoard;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository
{
	public interface IQuestRepository : IGenericRepository<Quest>
	{
		Task<Quest?> GetQuestById(string Id);
        Task<PagedList<Quest>> GetAllQuests(string? title, int? questType, int? type, int? questValue,
        string userId, int page, int eachPage);
        Task<QuestStatisticDto> GetQuestStatistic();
    }
}
