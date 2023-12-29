using Microsoft.Extensions.Caching.Memory;
using SpendingTracker.GenericSubDomain.User.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.GenericSubDomain.User.Internal
{
    internal sealed class UserByTelegramContextFactory : IUserContextFactory
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IUserRepository _userRepository;
        private readonly ITelegramUserIdStore _telegramUserIdStore;

        public UserByTelegramContextFactory(
            IMemoryCache memoryCache,
            IUserRepository userRepository,
            ITelegramUserIdStore telegramUserIdStore)
        {
            _memoryCache = memoryCache;
            _userRepository = userRepository;
            _telegramUserIdStore = telegramUserIdStore;
        }

        public string Key => nameof(UserByTelegramContextFactory);

        public async Task<UserContext> CreateUserContextAsync(CancellationToken cancellationToken)
        {
            var telegramUserId = _telegramUserIdStore.Id!.Value;
            
            var result = await _memoryCache.GetOrCreateAsync<UserContext>(
                nameof(UserByTelegramContextFactory),
                async cacheEntry =>
                {
                    var userId = await _userRepository.GetIdByTelegramId(telegramUserId, cancellationToken);
                    
                    cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

                    return new UserContext(userId);
                });

            return result;
        }
    }
}