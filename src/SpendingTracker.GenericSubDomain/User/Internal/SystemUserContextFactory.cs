using Microsoft.Extensions.Caching.Memory;
using SpendingTracker.GenericSubDomain.User.Abstractions;

namespace SpendingTracker.GenericSubDomain.User.Internal
{
    internal sealed class SystemUserContextFactory : IUserContextFactory
    {
        private readonly IMemoryCache _memoryCache;

        public SystemUserContextFactory(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public string Key => nameof(SystemUserContextFactory);

        public async Task<UserContext> CreateUserContextAsync(CancellationToken cancellationToken)
        {
            var result = await _memoryCache.GetOrCreateAsync<UserContext>(
                nameof(SystemUserContextFactory), cacheEntry =>
                {
                    cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

                    var systemUserId = Domain.User.SystemUserId;
                    return Task.FromResult(new UserContext(systemUserId));
                });

            return result;
        }
    }
}