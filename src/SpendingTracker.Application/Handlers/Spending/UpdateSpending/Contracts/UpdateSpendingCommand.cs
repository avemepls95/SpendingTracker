using SpendingTracker.Application.Handlers.Common;
using SpendingTracker.Domain;

namespace SpendingTracker.Application.Handlers.Spending.UpdateSpending.Contracts;

public class UpdateSpendingCommand : ISpendingTrackerCommand
{
    public Guid Id { get; init; }
    
    public double Amount { get; init; }

    public DateTimeOffset Date { get; init; }

    public string Description { get; init; }

    public Guid CurrencyId { get; set; }
    
    public ActionSource ActionSource { get; init; }
}