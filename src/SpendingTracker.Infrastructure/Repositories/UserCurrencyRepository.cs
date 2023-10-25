using Microsoft.EntityFrameworkCore;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored;
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

    public async Task ChangeUserCurrency(UserKey userId, string currencyCode, CancellationToken cancellationToken)
    {
        var storedCurrency = await _dbContext.Set<StoredCurrency>().FirstOrDefaultAsync(
            c => !c.IsDeleted && c.Code.ToUpper() == currencyCode,
            cancellationToken);

        if (storedCurrency is null)
        {
            throw new ArgumentException($"Не найдена валюта с кодом {currencyCode}");
        }

        var user = await _dbContext.Set<StoredUser>().FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user is null)
        {
            throw new ArgumentException($"Не найден пользователь с идентификатором {userId}");
        }

        user.Currency = storedCurrency;
    }
}