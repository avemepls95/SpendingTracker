using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SpendingTracker.GenericSubDomain.User.Abstractions;

namespace SpendingTracker.GenericSubDomain.User.Internal
{
    internal sealed class SystemUserContextFactory : IUserContextFactory
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IOptionsMonitor<SystemUserContextOptions> _options;

        public SystemUserContextFactory(IMemoryCache memoryCache, IOptionsMonitor<SystemUserContextOptions> options)
        {
            _memoryCache = memoryCache;
            _options = options;
        }

        public string Key => nameof(SystemUserContextFactory);

        public async Task<UserContext> CreateUserContextAsync(CancellationToken cancellationToken)
        {
            var result = await _memoryCache.GetOrCreateAsync<UserContext>(
                nameof(SystemUserContextFactory), cacheEntry =>
                {
                    cacheEntry.AbsoluteExpirationRelativeToNow = _options.CurrentValue.CacheAbsoluteExpirationRelativeToNow;

                    var systemUserId = Domain.User.SystemUserId;
                    return Task.FromResult(new UserContext(systemUserId));
                });

            return result;
        }
    }
}