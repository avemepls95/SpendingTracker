using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;
using SpendingTracker.Domain.Categories;
using SpendingTracker.Infrastructure.Abstractions.Models.Request;

namespace SpendingTracker.Infrastructure.Abstractions.Repositories
{
    public interface ISpendingRepository
    {
        Task CreateAsync(Spending spending, CancellationToken cancellationToken = default);

        Task<Spending[]> GetUserSpendingsInRange(
            UserKey userKey,
            DateOnly dateFrom,
            DateOnly dateTo,
            CancellationToken cancellationToken = default);

        Task<Spending[]> GetUserSpendings(
            UserKey userKey,
            int offset,
            int count,
            CancellationToken cancellationToken = default);

        Task AddExistToCategories(
            Guid spendingId,
            Category[] categories,
            CancellationToken cancellationToken = default);

        Task DeleteLastUserSpending(UserKey userId, CancellationToken cancellationToken = default);

        Task DeleteSpending(Guid spendingId, CancellationToken cancellationToken = default);

        Task UpdateSpending(UpdateSpendingModel model, CancellationToken cancellationToken = default);
    }
}