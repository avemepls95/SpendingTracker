using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;
using SpendingTracker.Domain.Accounts;

namespace SpendingTracker.Application.Handlers.Account.UpdateAccount.Contracts;

public class UpdateAccountCommand : ICommand
{
    public Guid Id { get; init; }
    public AccountTypeEnum Type { get; init; }
    public string Name { get; init; }
    public Guid CurrencyId { get; init; }
    public double Amount { get; init; }
}