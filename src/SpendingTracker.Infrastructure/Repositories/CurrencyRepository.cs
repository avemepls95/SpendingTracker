using Microsoft.EntityFrameworkCore;
using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.Infrastructure.Factories.Abstractions;

namespace SpendingTracker.Infrastructure.Repositories;

internal class CurrencyRepository : ICurrencyRepository
{
    private readonly MainDbContext _dbContext;
    private readonly ICurrencyFactory _currencyFactory;

    public CurrencyRepository(MainDbContext dbContext, ICurrencyFactory currencyFactory)
    {
        _dbContext = dbContext;
        _currencyFactory = currencyFactory;
    }

    public Task<Currency> GetDefaultAsync(CancellationToken cancellationToken)
    {
        return _dbContext.Set<StoredCurrency>()
            .Where(c => !c.IsDeleted && c.IsDefault)
            .Select(c => _currencyFactory.Create(c))
            .FirstAsync(cancellationToken);
    }

    public Task<Currency[]> GetAll(CancellationToken cancellationToken)
    {
        return _dbContext.Set<StoredCurrency>()
            .Where(c => !c.IsDeleted)
            .Select(c => _currencyFactory.Create(c))
            .ToArrayAsync(cancellationToken);
    }

    public Task<bool> IsExistsById(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Set<StoredCurrency>()
            .AnyAsync(c => c.Id == id && !c.IsDeleted,
                cancellationToken);
    }
}