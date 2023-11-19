using SpendingTracker.Application.Handlers.Common;
using SpendingTracker.Domain;

namespace SpendingTracker.Application.Handlers.Category.UpdateCategory.Contracts;

public class UpdateCategoryCommand : ISpendingTrackerCommand
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public ActionSource ActionSource { get; init; }
}