using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

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

        public async Task<IUserContext> CreateUserContextAsync(CancellationToken cancellationToken)
        {
            var result = await _memoryCache.GetOrCreateAsync<IUserContext>(
                nameof(SystemUserContextFactory),
                async cacheEntry =>
                {
                    cacheEntry.AbsoluteExpirationRelativeToNow = _options.CurrentValue.CacheAbsoluteExpirationRelativeToNow;

                    var user = Common.User.Default;
                    return new SystemUserContext(user);
                });

            return result;
        }
    }
}