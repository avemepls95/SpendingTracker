namespace SpendingTracker.Infrastructure.Abstractions.Models.Request;

public class UpdateSpendingModel
{
    public Guid Id { get; init; }
    
    public double Amount { get; init; }

    public DateTimeOffset Date { get; init; }

    public Guid CurrencyId { get; init; }

    public string Description { get; init; }
}