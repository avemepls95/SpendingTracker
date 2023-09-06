using FluentValidation;
using SpendingTracker.Application.Handlers.User.CreateUserByTelegramId.Contracts;

namespace SpendingTracker.Application.Handlers.User.CreateUserByTelegramId.Validators;

internal class CreateUserByTelegramCommandValidator : AbstractValidator<CreateUserByTelegramCommand>
{
    public CreateUserByTelegramCommandValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty();
        RuleFor(c => c.TelegramUserId).NotEmpty();
    }
}