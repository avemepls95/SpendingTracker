using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Spending.RemoveSpendingFromCategory.Contracts;

public class RemoveSpendingFromCategoryCommand : ICommand
{
    public Guid SpendingId { get; set; }
    public Guid CategoryId { get; set; }
}