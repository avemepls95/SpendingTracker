namespace SpendingTracker.Infrastructure.Abstractions
{
    public interface IUnitOfWork
    {
        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}