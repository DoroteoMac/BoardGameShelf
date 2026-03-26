namespace BoardGameShelf.Services.Cache;

/// <summary>
/// Provides generic caching operations.
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Returns a cached value for the given key, or creates and caches it using the factory if not present.
    /// </summary>
    Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan expiration);

    /// <summary>
    /// Removes a cached value by its key.
    /// </summary>
    void Remove(string key);
}
