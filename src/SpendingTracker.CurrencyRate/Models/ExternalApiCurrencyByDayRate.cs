namespace SpendingTracker.CurrencyRate.Models;

internal class ExternalApiCurrencyByDayRate
{
    public string SourceCode { get; init; }
    public string TargetCode { get; init; }
    public decimal Coefficient { get; init; }
    public DateOnly Date { get; set; }
}