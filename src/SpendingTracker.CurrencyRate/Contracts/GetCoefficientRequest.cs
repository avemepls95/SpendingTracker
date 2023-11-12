namespace SpendingTracker.CurrencyRate.Contracts;

public class GetCoefficientRequest
{
    public DateOnly Date { get; init; }
    public Guid CurrencyIdFrom { get; init; }
    public Guid CurrencyIdTo { get; init; }
}