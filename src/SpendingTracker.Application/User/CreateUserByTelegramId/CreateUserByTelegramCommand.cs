using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.User.CreateUserByTelegramId;

public class CreateUserByTelegramCommand : ICommand
{
    public long TelegramUserId { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
}