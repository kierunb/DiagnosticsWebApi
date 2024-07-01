using DiagnosticsWebApi.Entities;

namespace DiagnosticsWebApi.Services
{
    public interface IUserProfileCache
    {
        UserProfile GetOrAdd(Guid id, Func<Guid, UserProfile> valueProvider);
    }
}