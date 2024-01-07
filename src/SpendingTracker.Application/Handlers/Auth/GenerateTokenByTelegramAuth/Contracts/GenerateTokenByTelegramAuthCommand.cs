using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Auth.GenerateTokenByTelegramAuth.Contracts;

public class GenerateTokenByTelegramAuthCommand : ICommand<GenerateTokenByTelegramAuthResponse>
{
    public long UserId { get; init; }
    public string FirstName { get; init; } = null!;
    public string? LastName { get; init; }
    public string? UserName { get; init; }
    public string CheckString { get; init; } = null!;
    public TelegramAuthType AuthType { get; init; }
}