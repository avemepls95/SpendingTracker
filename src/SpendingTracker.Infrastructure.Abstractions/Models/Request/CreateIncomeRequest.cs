namespace SpendingTracker.Infrastructure.Abstractions.Models.Request;

public class CreateIncomeRequest
{
    public double Amount { get; init; }
    public DateTimeOffset Date { get; init; }
    public string Description { get; init; }
    public Guid? AccountId { get; init; }
}