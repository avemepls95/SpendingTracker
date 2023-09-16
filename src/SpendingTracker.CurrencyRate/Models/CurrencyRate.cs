namespace SpendingTracker.CurrencyRate.Models;

internal class CurrencyRate
{
    public string SourceCode { get; init; }
    public string TargetCode { get; init; }
    public decimal Coefficient { get; init; }
}