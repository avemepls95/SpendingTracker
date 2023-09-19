namespace SpendingTracker.Application.Handlers.Spending.GetSpendings.Contracts;

public class GetSpendingsResponseItem
{
    public Guid Id { get; init; }
    public double Amount { get; init; }
    public CurrencyDto Currency { get; init; }
    public DateTimeOffset Date { get; init; }
    public string Description { get; init; }
    public DateTimeOffset CreateDate { get; set; }
}