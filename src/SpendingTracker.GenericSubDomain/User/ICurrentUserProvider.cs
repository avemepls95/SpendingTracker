namespace SpendingTracker.GenericSubDomain.User
{
    public interface ICurrentUserProvider
    {
        Task<Domain.User> Get(CancellationToken cancellationToken = default);
    }
}