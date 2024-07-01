using DiagnosticsWebApi.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace DiagnosticsWebApi.Services;

public class BetterUserProfileCache(IMemoryCache cache) : IUserProfileCache
{
    public UserProfile GetOrAdd(Guid id, Func<Guid, UserProfile> valueProvider)
    {
        if (cache.TryGetValue(id, out UserProfile userProfile))
        {
            userProfile = valueProvider(id);

            // Limit the cache entry's lifetime by both size and timespan
            var options = new MemoryCacheEntryOptions()
                .SetSize(15 * 1024)
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            cache.Set(id, userProfile, options);
        }

        return userProfile;
    }
}
