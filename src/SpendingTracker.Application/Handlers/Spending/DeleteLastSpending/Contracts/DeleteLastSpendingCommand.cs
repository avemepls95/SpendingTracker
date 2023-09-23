using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Spending.DeleteLastSpending.Contracts;

public class DeleteLastSpendingCommand : ICommand
{
    public UserKey UserId { get; init; }
}