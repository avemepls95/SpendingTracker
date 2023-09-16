namespace SpendingTracker.CurrencyRate.Abstractions;

internal interface IRatesProvider
{
    Task<Models.CurrencyRate[]> Get(
        string baseCode,
        string[] codes,
        CancellationToken cancellationToken = default);
}