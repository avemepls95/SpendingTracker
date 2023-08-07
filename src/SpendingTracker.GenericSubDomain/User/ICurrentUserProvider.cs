namespace SpendingTracker.GenericSubDomain.User
{
    public interface ICurrentUserProvider
    {
        Task<Common.User> Get(CancellationToken cancellationToken = default);
    }
}