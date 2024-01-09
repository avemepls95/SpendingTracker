using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;
using SpendingTracker.Domain.Categories;
using SpendingTracker.Infrastructure.Abstractions.Models.Request;
using SpendingTracker.Infrastructure.Abstractions.Repositories.Models;

namespace SpendingTracker.Infrastructure.Abstractions.Repositories
{
    public interface ISpendingRepository
    {
        Task CreateAsync(CreateSpendingModel createModel, CancellationToken cancellationToken = default);

        Task<Spending[]> GetUserSpendingsInRange(
            UserKey userKey,
            DateOnly dateFrom,
            DateOnly dateTo,
            CancellationToken cancellationToken = default);
        
        Task<Spending[]> GetUserSpendingsByCategoryInRange(
            Guid categoryId,
            DateOnly dateFrom,
            DateOnly dateTo,
            CancellationToken cancellationToken = default);

        Task<Spending[]> GetUserSpendings(
            UserKey userKey,
            int offset,
            int count,
            string? searchString,
            bool onlyWithoutCategories = false,
            CancellationToken cancellationToken = default);
        
        Task<Spending> GetUserSpendingById(
            Guid id,
            CancellationToken cancellationToken = default);

        Task AddExistToCategories(
            Guid spendingId,
            Category[] categories,
            CancellationToken cancellationToken = default);

        Task DeleteLastUserSpending(UserKey userId, CancellationToken cancellationToken = default);

        Task DeleteSpending(Guid spendingId, CancellationToken cancellationToken = default);

        Task UpdateSpending(UpdateSpendingModel model, CancellationToken cancellationToken = default);

        Task<Spending> GetById(Guid id, CancellationToken cancellationToken = default);

        Task AddToCategory(Guid spendingId, Guid categoryId, CancellationToken cancellationToken = default);

        Task RemoveFromCategory(Guid spendingId, Guid categoryId, CancellationToken cancellationToken = default);

        Task<bool> IsSpendingHasCategory(
            Guid spendingId,
            Guid categoryId,
            CancellationToken cancellationToken = default);
    }
}