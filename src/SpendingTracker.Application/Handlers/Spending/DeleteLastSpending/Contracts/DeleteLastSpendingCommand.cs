using SpendingTracker.Application.Handlers.Common;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;

namespace SpendingTracker.Application.Handlers.Spending.DeleteLastSpending.Contracts;

public class DeleteLastSpendingCommand : ISpendingTrackerCommand
{
    public UserKey UserId { get; init; }
    public ActionSource ActionSource { get; init; }
}