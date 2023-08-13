using Microsoft.EntityFrameworkCore;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Model;
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
            .Where(c => c.IsDefault)
            .Select(c => _currencyFactory.Create(c))
            .FirstAsync(cancellationToken);
    }
}