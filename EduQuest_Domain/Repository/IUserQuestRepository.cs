using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;


namespace EduQuest_Domain.Repository;

public interface IUserQuestRepository : IGenericRepository<UserQuest>
{
    Task<bool> AddNewQuestToAllUserQuest(Quest newQuest);
}
