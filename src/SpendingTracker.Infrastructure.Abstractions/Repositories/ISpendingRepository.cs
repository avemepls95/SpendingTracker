using SpendingTracker.Domain;

namespace SpendingTracker.Infrastructure.Abstractions.Repositories
{
    public interface ISpendingRepository
    {
        Task CreateAsync(Spending ticket, CancellationToken cancellationToken = default);
    }
}