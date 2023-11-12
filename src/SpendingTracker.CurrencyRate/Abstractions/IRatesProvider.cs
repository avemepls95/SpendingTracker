namespace SpendingTracker.CurrencyRate.Abstractions;

internal interface IRatesProvider
{
    Task<Models.ExternalApiCurrencyRate[]> Get(
        string baseCode,
        string[] codes,
        CancellationToken cancellationToken = default);
}