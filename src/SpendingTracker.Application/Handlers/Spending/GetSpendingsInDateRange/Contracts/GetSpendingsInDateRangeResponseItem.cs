namespace SpendingTracker.Application.Handlers.Spending.GetSpendingsInDateRange.Contracts;

public class GetSpendingsInDateRangeResponseItem
{
    public Guid Id { get; init; }
    public double Amount { get; init; }
    public Guid CurrencyId { get; init; }
    public DateTimeOffset Date { get; init; }
    public string Description { get; init; }
    public DateTimeOffset CreateDate { get; set; }
}