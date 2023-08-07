namespace SpendingTracker.GenericSubDomain.User.Internal
{
    internal sealed class CurrentUserProvider : ICurrentUserProvider
    {
        private readonly IUserContextFactory _userContextFactory;

        public CurrentUserProvider(IUserContextFactory userContextFactory)
        {
            _userContextFactory = userContextFactory;
        }

        public async Task<Common.User> Get(CancellationToken cancellationToken = default)
        {
            var userContext = await _userContextFactory.CreateUserContextAsync(cancellationToken);
            var currentUser = userContext.CurrentUser;

            return currentUser;
        }
    }
}