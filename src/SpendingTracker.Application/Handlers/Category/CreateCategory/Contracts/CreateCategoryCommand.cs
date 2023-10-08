using SpendingTracker.Application.Handlers.Common;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;

namespace SpendingTracker.Application.Handlers.Category.CreateCategory.Contracts;

public class CreateCategoryCommand : ISpendingTrackerCommand
{
    public UserKey UserId { get; set; }
    public string Title { get; set; }
    public ActionSource ActionSource { get; init; }
}