namespace SpendingTracker.GenericSubDomain.User
{
    public interface IUserContextFactory
    {
        string Key { get; }

        Task<IUserContext> CreateUserContextAsync(CancellationToken cancellationToken = default);
    }
}