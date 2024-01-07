using SpendingTracker.Application.Handlers.Auth.GenerateTokenByTelegramAuth.Contracts;

namespace SpendingTracker.Application.Handlers.Auth.GenerateTokenByTelegramAuth._Internal;

internal interface ITelegramHashValidator
{
    bool IsValid(string checkString, TelegramAuthType authType);
}