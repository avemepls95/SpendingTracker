using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Auth.GenerateTokenByTelegramAuth.Contracts;

public class GenerateTokenByTelegramAuthCommand : ICommand<GenerateTokenByTelegramAuthResponse>
{
    public string AuthDateAsString { get; init; } = null!;
    public long UserId { get; init; }
    public string FirstName { get; init; } = null!;
    public string? LastName { get; init; }
    public string? UserName { get; init; }
    public string? PhotoUrl { get; init; }
    public string Hash { get; init; } = null!;
}