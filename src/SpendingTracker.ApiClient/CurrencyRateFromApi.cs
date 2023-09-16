namespace SpendingTracker.ApiClient;

public class CurrencyRateFromApi
{
    public string SourceCode { get; init; }
    public string TargetCode { get; init; }
    public decimal Coefficient { get; init; }
}