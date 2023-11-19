namespace SpendingTracker.ApiClient;

public class CurrencyRateByDayFromApi
{
    public string SourceCode { get; init; }
    public string TargetCode { get; init; }
    public decimal Coefficient { get; init; }
    public DateOnly Date { get; init; }
}