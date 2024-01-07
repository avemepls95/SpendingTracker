using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.User.CreateUserByTelegramId.Contracts;

public class CreateUserByTelegramCommand : ICommand<UserKey>
{
    public long TelegramUserId { get; init; }
    public string FirstName { get; init; }
    public string? LastName { get; init; }
    public string? UserName { get; init; }
}