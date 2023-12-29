using FluentValidation;
using SpendingTracker.Application.Handlers.Auth.GenerateTokenByTelegramAuth.Contracts;

namespace SpendingTracker.Application.Handlers.Auth.GenerateTokenByTelegramAuth.Validators;

internal class GenerateTokenByTelegramAuthCommandValidator : AbstractValidator<GenerateTokenByTelegramAuthCommand>
{
    public GenerateTokenByTelegramAuthCommandValidator()
    {
        RuleFor(c => c.Hash).NotEmpty();
        RuleFor(c => c.AuthDateAsString).NotEmpty();
        RuleFor(c => c.FirstName).NotEmpty();
        RuleFor(c => c.UserId).NotEmpty();
    }
}