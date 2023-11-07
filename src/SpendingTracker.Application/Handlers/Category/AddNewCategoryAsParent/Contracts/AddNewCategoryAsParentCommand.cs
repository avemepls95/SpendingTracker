using SpendingTracker.Application.Handlers.Common;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;

namespace SpendingTracker.Application.Handlers.Category.AddNewCategoryAsParent.Contracts;

public class AddNewCategoryAsParentCommand : ISpendingTrackerCommand
{
    public UserKey UserId { get; set; }
    public Guid ChildId { get; set; }
    public string NewParentTitle { get; set; }
    public ActionSource ActionSource { get; init; }
}