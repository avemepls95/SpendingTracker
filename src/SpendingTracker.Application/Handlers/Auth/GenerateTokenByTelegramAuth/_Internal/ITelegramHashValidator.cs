namespace SpendingTracker.Application.Handlers.Auth.GenerateTokenByTelegramAuth._Internal;

internal interface ITelegramHashValidator
{
    bool IsValid(
        string hash,
        string authDateAsString,
        string firstName,
        string? lastName,
        long userId,
        string? photoUrl,
        string? userName);
}