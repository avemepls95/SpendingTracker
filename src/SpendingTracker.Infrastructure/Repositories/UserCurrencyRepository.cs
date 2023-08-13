using Microsoft.EntityFrameworkCore;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Model;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.Infrastructure.Factories.Abstractions;

namespace SpendingTracker.Infrastructure.Repositories;

internal class UserCurrencyRepository : IUserCurrencyRepository
{
    private readonly MainDbContext _dbContext;
    private readonly ICurrencyFactory _currencyFactory;

    public UserCurrencyRepository(MainDbContext dbContext, ICurrencyFactory currencyFactory)
    {
        _dbContext = dbContext;
        _currencyFactory = currencyFactory;
    }

    public async Task<Currency> Get(UserKey userKey, CancellationToken cancellationToken)
    {
        var dbCurrency = await _dbContext.Set<StoredUser>()
            .Where(u => u.Id == userKey)
            .Select(u => u.Currency)
            .FirstOrDefaultAsync(cancellationToken);

        if (dbCurrency == null)
        {
            throw new ArgumentNullException(nameof(userKey));
        }

        return _currencyFactory.Create(dbCurrency);
    }

    public Task<Currency> GetDefaultAsync(CancellationToken cancellationToken)
    {
        return _dbContext.Set<StoredCurrency>()
            .Where(c => c.IsDefault)
            .Select(c => _currencyFactory.Create(c))
            .FirstAsync(cancellationToken);
    }
}