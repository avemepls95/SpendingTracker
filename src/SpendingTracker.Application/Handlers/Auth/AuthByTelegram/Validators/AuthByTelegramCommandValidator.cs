using FluentValidation;
using SpendingTracker.Application.Handlers.Auth.AuthByTelegram.Contracts;

namespace SpendingTracker.Application.Handlers.Auth.AuthByTelegram.Validators;

internal class AuthByTelegramCommandValidator : AbstractValidator<AuthByTelegramCommand>
{
    public AuthByTelegramCommandValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty();
        RuleFor(c => c.UserId).NotEmpty();
    }
}