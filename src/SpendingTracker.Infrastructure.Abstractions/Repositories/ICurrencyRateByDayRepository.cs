using SpendingTracker.Infrastructure.Abstractions.Repositories.Models;

namespace SpendingTracker.Infrastructure.Abstractions.Repositories;

public interface ICurrencyRateByDayRepository
{
    public Task Create(
        DateTimeOffset date,
        Guid targetId,
        decimal coefficient,
        CancellationToken cancellationToken = default);

    public Task<bool> ExistsAnyInThisDay(DateTimeOffset day, CancellationToken cancellationToken = default);

    public Task<CurrencyRateByDayShortModel[]> GetRatesByDays(
        DateOnly[] dates,
        CancellationToken cancellationToken = default);
    
    public Task<CurrencyRateByDayShortModel[]> GetRatesByNearestDay(
        DateOnly date,
        CancellationToken cancellationToken = default);
}