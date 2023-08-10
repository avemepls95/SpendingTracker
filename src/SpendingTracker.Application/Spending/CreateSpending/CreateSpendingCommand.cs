using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Spending.CreateSpending;

public class CreateSpendingCommand : ICommand
{
    public double Amount { get; set; }

    public UserKey UserKey { get; set; }

    public DateTimeOffset Date { get; set; }

    public string Description { get; set; }
}