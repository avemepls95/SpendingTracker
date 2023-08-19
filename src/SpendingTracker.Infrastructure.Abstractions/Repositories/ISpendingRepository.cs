using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;

namespace SpendingTracker.Infrastructure.Abstractions.Repositories
{
    public interface ISpendingRepository
    {
        Task CreateAsync(Spending spending, CancellationToken cancellationToken = default);

        Task<Spending[]> GetUserSpendings(
            UserKey userKey,
            DateTimeOffset? dateFrom,
            DateTimeOffset? dateTo,
            CancellationToken cancellationToken = default);
    }
}