using SpendingTracker.Common.Primitives;
using SpendingTracker.GenericSubDomain.User.Abstractions;

namespace SpendingTracker.GenericSubDomain.User.Internal
{
    internal sealed class CurrentUserIdProvider : ICurrentUserIdProvider
    {
        private readonly IUserContextFactory _userContextFactory;

        public CurrentUserIdProvider(IUserContextFactory userContextFactory)
        {
            _userContextFactory = userContextFactory;
        }

        public async Task<UserKey> Get(CancellationToken cancellationToken = default)
        {
            var userContext = await _userContextFactory.CreateUserContextAsync(cancellationToken);
            var currentUser = userContext.UserId;

            return currentUser;
        }
    }
}