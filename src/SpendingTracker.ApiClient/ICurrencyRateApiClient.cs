namespace SpendingTracker.ApiClient;

public interface ICurrencyRateApiClient
{
    Task<ExternalCallResult<CurrencyRateFromApi[]>> GetByCodes(
        string baseCode,
        string[] codes,
        CancellationToken cancellationToken = default);
}