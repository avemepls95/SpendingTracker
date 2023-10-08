using SpendingTracker.Application.Handlers.Common;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;

namespace SpendingTracker.Application.Handlers.Category.DeleteCategory.Contracts;

public class DeleteCategoryCommand : ISpendingTrackerCommand
{
    public UserKey InitiatorId { get; init; }
    public Guid Id { get; init; }
    public ActionSource ActionSource { get; init; }
}