using SpendingTracker.GenericSubDomain.User.Abstractions;
using SpendingTracker.GenericSubDomain.User.Internal;

namespace SpendingTracker.GenericSubDomain.User
{
    internal sealed class UserContextFactoryProvider : IUserContextFactoryProvider
    {
        private readonly IEnumerable<IUserContextFactory> _userContextFactories;
        private readonly ITelegramUserIdStore _telegramUserIdStore;

        public UserContextFactoryProvider(
            IEnumerable<IUserContextFactory> userContextFactories,
            ITelegramUserIdStore telegramUserIdStore)
        {
            _userContextFactories = userContextFactories;
            _telegramUserIdStore = telegramUserIdStore;
        }

        public IUserContextFactory GetSuitableFactory()
        {
            if (_telegramUserIdStore.Id.HasValue)
            {
                var telegramUserContextFactory = _userContextFactories.First(f => f.Key == nameof(UserByTelegramContextFactory));
                return telegramUserContextFactory;
            }

            return _userContextFactories.First(f => f.Key == nameof(SystemUserContextFactory));
        }
    }
}