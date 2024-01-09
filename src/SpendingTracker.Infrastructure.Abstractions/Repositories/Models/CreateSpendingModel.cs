using SpendingTracker.Domain;

namespace SpendingTracker.Infrastructure.Abstractions.Repositories.Models;

public class CreateSpendingModel
{
    public double Amount { get; init; }
    public Guid CurrencyId { get; init; }
    public DateTimeOffset Date { get; init; }
    public string Description { get; init; }
    public ActionSource ActionSource { get; init; }
}