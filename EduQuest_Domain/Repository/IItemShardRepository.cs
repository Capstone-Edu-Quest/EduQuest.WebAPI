using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;


namespace EduQuest_Domain.Repository;

public interface IItemShardRepository : IGenericRepository<ItemShards>
{
    Task<ItemShards?> GetItemShardsByTagId(string tagId, string userId);
    Task<List<ItemShards>?> GetItemShardsByUserId(string userId);
}
