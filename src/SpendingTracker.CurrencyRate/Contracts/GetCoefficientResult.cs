namespace SpendingTracker.CurrencyRate.Contracts;

public class GetCoefficientResult
{
    public DateOnly Date { get; init; }
    public Guid CurrencyFrom { get; init; }
    public Guid CurrencyTo { get; init; }
    public decimal Coefficient { get; set; }
}