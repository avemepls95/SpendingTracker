using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Auth.RefreshTokenByTelegramAuth.Contracts;

public class RefreshTokenByTelegramAuthCommand : ICommand<RefreshTokenByTelegramAuthResponse>
{
    public string RefreshToken { get; init; }
    public UserKey UserId { get; set; }
}