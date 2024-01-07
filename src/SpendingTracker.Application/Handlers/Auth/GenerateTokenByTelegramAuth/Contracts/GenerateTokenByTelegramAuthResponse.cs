using SpendingTracker.BearerTokenAuth;
using SpendingTracker.Common.Primitives;

namespace SpendingTracker.Application.Handlers.Auth.GenerateTokenByTelegramAuth.Contracts;

public class GenerateTokenByTelegramAuthResponse
{
    public TokenInformation TokenInformation { get; set; }
    public UserKey Id { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
}