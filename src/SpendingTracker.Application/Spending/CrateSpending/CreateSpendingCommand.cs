using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Spending.CrateSpending;

public class CreateSpendingCommand : ICommand
{
    public double Amount { get; set; }

    public string Currency { get; set; }

    public DateTimeOffset Date { get; set; }

    public string Description { get; set; }
}