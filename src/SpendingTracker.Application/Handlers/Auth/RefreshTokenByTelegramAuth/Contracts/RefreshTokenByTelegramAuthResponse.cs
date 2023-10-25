namespace SpendingTracker.Application.Handlers.Auth.RefreshTokenByTelegramAuth.Contracts;

public class RefreshTokenByTelegramAuthResponse
{
    public string AccessToken { get; set; }
    public DateTimeOffset ExpiresIn { get; set; }
    public string RefreshToken { get; set; }
}