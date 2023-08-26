using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;

namespace SpendingTracker.Infrastructure.Abstractions.Repositories;

public interface IUserCurrencyRepository
{
    Task<Currency> Get(UserKey userKey, CancellationToken cancellationToken = default);
    Task<Currency> GetDefaultAsync(CancellationToken cancellationToken = default);
    Task ChangeUserCurrency(UserKey userId, string currencyCode, CancellationToken cancellationToken = default);
}