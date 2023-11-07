using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Spending.AddSpendingToNewCategory.Contracts;

public class AddSpendingToNewCategoryCommand : ICommand
{
    public UserKey UserId { get; init; }
    public Guid SpendingId { get; init; }
    public string NewCategoryTitle { get; init; }
}