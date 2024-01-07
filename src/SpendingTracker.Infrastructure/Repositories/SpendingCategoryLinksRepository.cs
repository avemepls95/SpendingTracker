using Microsoft.EntityFrameworkCore;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored.Categories;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.Infrastructure.Abstractions.Repositories.Models;

namespace SpendingTracker.Infrastructure.Repositories;

internal class SpendingCategoryLinksRepository : ISpendingCategoryLinksRepository
{
    private readonly MainDbContext _dbContext;

    public SpendingCategoryLinksRepository(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SpendingCategoryLink[]> GetBySpendings(Guid[] spendingIds, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Set<StoredSpendingCategoryLink>()
            .Where(l => spendingIds.Contains(l.SpendingId))
            .Select(l => new SpendingCategoryLink
            {
                SpendingId = l.SpendingId,
                CategoryId = l.CategoryId
            })
            .ToArrayAsync(cancellationToken);

        return result;
    }
}