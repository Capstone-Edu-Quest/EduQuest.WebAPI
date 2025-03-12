using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository.Generic;
using static EduQuest_Domain.Enums.QuestEnum;


namespace EduQuest_Domain.Repository;

public interface IUserQuestRepository : IGenericRepository<UserQuest>
{
    Task<bool> AddNewQuestToAllUserQuest(Quest newQuest);
    Task<bool> UpdateAllUserQuest(Quest updatedQuest);
    Task<PagedList<UserQuest>> GetAllUserQuests(string? title, string? description, int? pointToComplete,
    int? type, DateTime? startDate, DateTime? dueDate, int page, int pageSize, string userId);
    Task<List<Reward>> GetUserQuestRewardAsync(List<string> rewardIds);

    Task<bool> UpdateUserQuestsProgress(string userId, QuestType questType, int addedPoint);
}
