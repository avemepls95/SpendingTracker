using SpendingTracker.Application.Handlers.Common;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;

namespace SpendingTracker.Application.Handlers.Income.DeleteLast.Contracts;

public class DeleteLastIncomeCommand : ISpendingTrackerCommand
{
    public UserKey UserId { get; init; }
    public ActionSource ActionSource { get; init; }
}