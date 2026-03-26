using Microsoft.Extensions.Caching.Memory;

namespace BoardGameShelf.Services.Cache;

/// <summary>
/// In-memory cache implementation using IMemoryCache.
/// </summary>
public class CacheService(IMemoryCache cache) : ICacheService
{
    /// <summary>
    /// Returns a cached value for the given key, or creates and caches it using the factory if not present.
    /// </summary>
    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan expiration)
    {
        if (cache.TryGetValue(key, out T? cached) && cached is not null)
            return cached;

        var value = await factory();
        cache.Set(key, value, expiration);
        return value;
    }

    /// <summary>
    /// Removes a cached value by its key.
    /// </summary>
    public void Remove(string key) => cache.Remove(key);
}
