using FluentValidation;
using SpendingTracker.Application.Handlers.Auth.RefreshTokenByTelegramAuth.Contracts;

namespace SpendingTracker.Application.Handlers.Auth.RefreshTokenByTelegramAuth.Validators;

internal class GenerateTokenByTelegramAuthCommandValidator : AbstractValidator<RefreshTokenByTelegramAuthCommand>
{
    public GenerateTokenByTelegramAuthCommandValidator()
    {
        RuleFor(c => c.RefreshToken).NotEmpty();
        RuleFor(c => c.UserId).NotEmpty();
    }
}