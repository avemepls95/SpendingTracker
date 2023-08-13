using SpendingTracker.Domain;

namespace SpendingTracker.Infrastructure.Abstractions.Repositories;

public interface ICurrencyRepository
{
    Task<Currency> GetDefaultAsync(CancellationToken cancellationToken);
}