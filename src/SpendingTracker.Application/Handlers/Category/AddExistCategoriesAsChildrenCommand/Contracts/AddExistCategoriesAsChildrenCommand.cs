using SpendingTracker.Application.Handlers.Common;
using SpendingTracker.Domain;

namespace SpendingTracker.Application.Handlers.Category.AddExistCategoriesAsChildrenCommand.Contracts;

public class AddExistCategoriesAsChildrenCommand : ISpendingTrackerCommand
{
    public Guid ParentId { get; set; }
    public Guid[] ChildIds { get; set; }
    public ActionSource ActionSource { get; init; }
}