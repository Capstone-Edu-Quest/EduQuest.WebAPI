using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Models.PlatformStatisticDashBoard;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface ILevelRepository : IGenericRepository<Levels>
{
    Task<bool> IsLevelExist(int level);
    Task<PagedList<Levels>> GetLevelWithFiltersAsync(int? level, int? exp, int page, int eachPage);
    Task<IEnumerable<Levels>> GetByBatchLevelNumber(List<string> levelIds);
    Task ReArrangeLevelAfterDelete(int level);
    int GetExpByLevel(int level);
    Task<LevelExpStatisticDto> GetLevelExpStatistic();
    Task<int> DeleteRangeByListId(List<string> ids);
}
