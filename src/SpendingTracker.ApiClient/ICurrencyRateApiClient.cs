namespace SpendingTracker.ApiClient;

public interface ICurrencyRateApiClient
{
    Task<ExternalCallResult<CurrencyRateFromApi[]>> GetToday(
        string baseCode,
        string[] codes,
        CancellationToken cancellationToken = default);

    Task<ExternalCallResult<CurrencyRateByDayFromApi[]>> GetByDates(
        string baseCode,
        string[] codes,
        DateOnly[] dates,
        CancellationToken cancellationToken = default);
}