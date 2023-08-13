using SpendingTracker.Common.Primitives;

namespace SpendingTracker.GenericSubDomain.User
{
    public interface ICurrentUserIdProvider
    {
        Task<UserKey> Get(CancellationToken cancellationToken = default);
    }
}