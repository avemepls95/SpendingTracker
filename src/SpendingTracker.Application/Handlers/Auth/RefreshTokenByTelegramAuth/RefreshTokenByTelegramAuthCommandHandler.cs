using SpendingTracker.Application.Handlers.Auth.RefreshTokenByTelegramAuth.Contracts;
using SpendingTracker.BearerTokenAuth.Abstractions;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;

namespace SpendingTracker.Application.Handlers.Auth.RefreshTokenByTelegramAuth;

internal sealed class GenerateTokenByTelegramAuthCommandHandler
    : CommandHandler<RefreshTokenByTelegramAuthCommand, RefreshTokenByTelegramAuthResponse>
{
    private readonly ITokenGenerator _tokenGenerator;

    public GenerateTokenByTelegramAuthCommandHandler(ITokenGenerator tokenGenerator)
    {
        _tokenGenerator = tokenGenerator;
    }

    public override Task<RefreshTokenByTelegramAuthResponse> Handle(
        RefreshTokenByTelegramAuthCommand command,
        CancellationToken cancellationToken)
    {
        var refreshToken = _tokenGenerator.Refresh(
            command.RefreshToken,
            command.UserId.Value);
    
        return Task.FromResult(new RefreshTokenByTelegramAuthResponse
        {
            AccessToken = refreshToken.AccessToken,
            ExpiresIn = refreshToken.ExpiresIn,
            RefreshToken = refreshToken.RefreshToken
        });
    }
}