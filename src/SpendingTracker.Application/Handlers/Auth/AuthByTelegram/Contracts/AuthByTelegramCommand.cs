using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Auth.AuthByTelegram.Contracts;

public class AuthByTelegramCommand : ICommand<AuthByTelegramResponse>
{
    public long UserId { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
    public string? PhotoUrl { get; set; }
}