using SpendingTracker.Application.Handlers.Common;
using SpendingTracker.Domain;

namespace SpendingTracker.Application.Handlers.Spending.DeleteSpending.Contracts;

public class DeleteSpendingCommand : ISpendingTrackerCommand
{
    public Guid Id { get; init; }
    public ActionSource ActionSource { get; init; }
}