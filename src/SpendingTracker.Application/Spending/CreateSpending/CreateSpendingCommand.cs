using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;
using SpendingTracker.Domain;

namespace SpendingTracker.Application.Spending.CreateSpending;

public class CreateSpendingCommand : ICommand
{
    public double Amount { get; set; }

    public Currency Currency { get; set; }

    public DateTimeOffset Date { get; set; }

    public string Description { get; set; }
}