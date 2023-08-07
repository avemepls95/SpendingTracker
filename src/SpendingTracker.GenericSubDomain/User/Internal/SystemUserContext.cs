namespace SpendingTracker.GenericSubDomain.User.Internal
{
    internal sealed class SystemUserContext : IUserContext
    {
        public SystemUserContext(Common.User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            CurrentUser = user;
        }

        public Common.User CurrentUser { get; }
    }
}