namespace SpendingTracker.GenericSubDomain.User.Abstractions
{
    public interface IUserContextFactory
    {
        string Key { get; }

        Task<UserContext> CreateUserContextAsync(CancellationToken cancellationToken = default);
    }
}