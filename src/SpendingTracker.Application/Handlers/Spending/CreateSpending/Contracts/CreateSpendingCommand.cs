using SpendingTracker.Application.Handlers.Common;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;

namespace SpendingTracker.Application.Handlers.Spending.CreateSpending.Contracts;

public class CreateSpendingCommand : ISpendingTrackerCommand
{
    public double Amount { get; init; }

    public UserKey UserId { get; init; }

    public DateTimeOffset Date { get; init; }

    public string Description { get; init; }
    
    public ActionSource ActionSource { get; init; }
}