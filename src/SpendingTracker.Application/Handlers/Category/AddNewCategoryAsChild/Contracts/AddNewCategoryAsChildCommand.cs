using SpendingTracker.Application.Handlers.Common;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;

namespace SpendingTracker.Application.Handlers.Category.AddNewCategoryAsChild.Contracts;

public class AddNewCategoryAsChildCommand : ISpendingTrackerCommand
{
    public UserKey UserId { get; set; }
    public Guid ParentId { get; set; }
    public string ChildTitle { get; set; }
    public ActionSource ActionSource { get; init; }
}