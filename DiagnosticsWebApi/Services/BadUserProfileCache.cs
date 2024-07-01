using DiagnosticsWebApi.Entities;
using System.Collections.Concurrent;

namespace DiagnosticsWebApi.Services;

public class BadUserProfileCache() : IUserProfileCache
{
    private ConcurrentDictionary<Guid, UserProfile> _cache = new();

    public UserProfile GetOrAdd(Guid id, Func<Guid, UserProfile> valueProvider) => _cache.GetOrAdd(id, valueProvider);

}
