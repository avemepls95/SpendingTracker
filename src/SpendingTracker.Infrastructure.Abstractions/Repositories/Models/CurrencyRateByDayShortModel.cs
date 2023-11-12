namespace SpendingTracker.Infrastructure.Abstractions.Repositories.Models;

public class CurrencyRateByDayShortModel
{
    public DateTimeOffset Date { get; init; }
    public Guid Base { get; init; }
    public Guid Target { get; init; }
    public decimal Coefficient { get; init; }
}