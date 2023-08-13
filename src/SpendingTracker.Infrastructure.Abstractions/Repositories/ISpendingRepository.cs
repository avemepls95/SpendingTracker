using SpendingTracker.Domain;

namespace SpendingTracker.Infrastructure.Abstractions.Repositories
{
    public interface ISpendingRepository
    {
        Task CreateAsync(Spending spending, CancellationToken cancellationToken = default);
    }
}