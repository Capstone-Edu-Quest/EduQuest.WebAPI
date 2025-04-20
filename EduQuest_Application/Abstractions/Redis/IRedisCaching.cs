using StackExchange.Redis;

namespace EduQuest_Application.Abstractions.Redis;

public interface IRedisCaching
{
    /// <summary>
    /// Get value by key 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<T> GetAsync<T>(string key);

    /// <summary>
    /// set cache value with key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="TimeToLive"></param>
    /// <returns></returns>
    Task SetAsync<T>(string key, T value, double TimeToLive);

    /// <summary>
    /// remove value by key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task RemoveAsync(string key);

    Task<List<string>> SearchKeysAsync(string keyword);

    Task FlushByRelatedKey(string keyword);

    Task HashSetAsync(string key, HashEntry[] hashEntries, int TimeToLive);
    Task<string?> HashGetSpecificKeyAsync(string key, string field);

    Task<bool> DeleteKeyAsync(string key);
    Task<Dictionary<string, string>?> GetAllHashDataAsync(string key);

    Task AddToSortedSetAsync(string key, string member, double score);
    Task<long?> GetSortSetRankAsync(string key, string member);
    Task<List<(string member, double score, long rank)>> GetTopSortedSetAsync(string key, int topN);
}