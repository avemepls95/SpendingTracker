using Microsoft.EntityFrameworkCore;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Infrastructure.Repositories;

internal class UserCurrencyRepository : IUserCurrencyRepository
{
    private readonly MainDbContext _dbContext;

    public UserCurrencyRepository(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Currency> Get(UserKey userKey, CancellationToken cancellationToken)
    {
        var currency = await _dbContext.Set<User>()
            .Where(u => u.Id == userKey)
            .Select(u => u.Currency)
            .FirstOrDefaultAsync(cancellationToken);

        if (currency == null)
        {
            throw new ArgumentNullException(nameof(userKey));
        }

        return currency;
    }
}