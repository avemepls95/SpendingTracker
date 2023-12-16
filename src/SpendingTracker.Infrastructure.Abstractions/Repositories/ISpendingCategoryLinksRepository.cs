using SpendingTracker.Infrastructure.Abstractions.Repositories.Models;

namespace SpendingTracker.Infrastructure.Abstractions.Repositories;

public interface ISpendingCategoryLinksRepository
{
    Task<SpendingCategoryLink[]> GetBySpendings(Guid[] spendingIds, CancellationToken cancellationToken = default);
}