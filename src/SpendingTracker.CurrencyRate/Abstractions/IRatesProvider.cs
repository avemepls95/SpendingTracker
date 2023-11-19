namespace SpendingTracker.CurrencyRate.Abstractions;

internal interface IRatesProvider
{
    Task<Models.ExternalApiTodayCurrencyRate[]> GetToday(
        string baseCode,
        string[] codes,
        CancellationToken cancellationToken = default);

    Task<Models.ExternalApiCurrencyByDayRate[]> GetByDates(
        string baseCode,
        string[] codes,
        DateOnly[] dates,
        CancellationToken cancellationToken = default);
}