using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository
{
    public interface ITagRepository : IGenericRepository<Tag>
	{
        Task<Tag> GetTagByName(string name);
        Task<PagedList<Tag>> GetTagsWithFilters(string? Id, string? Name, int page, int eachPage, int? Type);
    }
}
