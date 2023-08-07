using SpendingTracker.GenericSubDomain.User.Internal;

namespace SpendingTracker.GenericSubDomain.User
{
    public sealed class UserContextFactoryProvider
    {
        private readonly IEnumerable<IUserContextFactory> _userContextFactories;

        public UserContextFactoryProvider(IEnumerable<IUserContextFactory> userContextFactories)
        {
            _userContextFactories = userContextFactories;
        }

        public IUserContextFactory GetSuitableFactory()
        {
            return _userContextFactories.First(f => f.Key == nameof(SystemUserContextFactory));
        }
    }
}