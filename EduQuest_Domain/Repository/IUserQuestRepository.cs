using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository.Generic;
using static EduQuest_Domain.Enums.QuestEnum;


namespace EduQuest_Domain.Repository;

public interface IUserQuestRepository : IGenericRepository<UserQuest>
{
    Task<bool> AddNewQuestToAllUserQuest(Quest newQuest);
    Task<bool> UpdateAllUserQuest(Quest updatedQuest);
    Task<PagedList<UserQuest>> GetAllUserQuests(string? title, int? questType, int? type, int? pointToComplete,
        DateTime? startDate, DateTime? dueDate, bool? isComplete, string userId, int page, int eachPage);

    Task<bool> UpdateUserQuestsProgress(string userId, QuestType questType, int addedPoint);
}
