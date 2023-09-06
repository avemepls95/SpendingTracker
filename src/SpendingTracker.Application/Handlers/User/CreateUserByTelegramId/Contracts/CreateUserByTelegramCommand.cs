using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.User.CreateUserByTelegramId.Contracts;

public class CreateUserByTelegramCommand : ICommand<UserKey>
{
    public long TelegramUserId { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
    public string? PhotoUrl { get; set; }
}