using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Account.DeleteAccount.Contracts;

public class DeleteAccountCommand : ICommand
{
    public Guid Id { get; init; }
}