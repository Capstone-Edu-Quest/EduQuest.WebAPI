using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface ILevelRewardRepository : IGenericRepository<LevelReward>
{
    Task<IEnumerable<LevelReward>> GetByIdToList(string id);
    Task<IEnumerable<LevelReward>> GetByLevelIdAsync(string levelId);
    void RemoveRangeAsync(IEnumerable<LevelReward> rewards);
}
