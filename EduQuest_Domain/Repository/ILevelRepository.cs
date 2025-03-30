using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface ILevelRepository : IGenericRepository<Level>
{
    Task<PagedList<Level>> GetLevelWithFiltersAsync(int? level, int? exp, int page, int eachPage);
    Task<IEnumerable<Level>> GetByBatchLevelNumber(List<string> levelIds);
}
