using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Spending.AddSpendingToExistCategory.Contracts;

public class AddSpendingToExistCategoryCommand : ICommand
{
    public UserKey UserId { get; init; }
    public Guid SpendingId { get; set; }
    public Guid CategoryId { get; set; }
}