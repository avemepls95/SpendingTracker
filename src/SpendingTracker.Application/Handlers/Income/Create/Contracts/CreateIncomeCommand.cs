using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Income.Create.Contracts;

public class CreateIncomeCommand : ICommand
{
    public double Amount { get; init; }
    public UserKey UserId { get; init; }
    public DateTimeOffset Date { get; init; }
    public string Description { get; init; }
    public Guid? AccountId { get; init; }
}