using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;
using SpendingTracker.Domain.Accounts;

namespace SpendingTracker.Application.Handlers.Account.CreateAccount.Contracts;

public class CreateAccountCommand : ICommand
{
    public UserKey UserId { get; set; }
    public AccountTypeEnum Type { get; set; }
    public string Name { get; set; }
    public Guid CurrencyId { get; set; }
    public double Amount { get; set; }
}