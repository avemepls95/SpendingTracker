using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Auth.AuthByTelegram.Contracts;

public class AuthByTelegramCommand : ICommand<AuthByTelegramResponse>
{
    public long UserId { get; init; }
    public string FirstName { get; init; }
    public string? LastName { get; init; }
    public string? UserName { get; init; }
    public string? PhotoUrl { get; init; }
}