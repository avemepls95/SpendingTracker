using SpendingTracker.Infrastructure.Abstractions.Model;

namespace SpendingTracker.Infrastructure.Abstractions.Repositories
{
    public interface ISpendingRepository
    {
        Task CreateAsync(StoredSpending spending, CancellationToken cancellationToken = default);
    }
}