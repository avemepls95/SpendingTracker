using FluentValidation;
using SpendingTracker.Application.Handlers.UserCurrency.ChangeUserCurrency.Contracts;

namespace SpendingTracker.Application.Handlers.UserCurrency.ChangeUserCurrency.Validators;

internal class ChangeUserCurrencyCommandValidator : AbstractValidator<ChangeUserCurrencyCommand>
{
    public ChangeUserCurrencyCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.UserId.Value).NotEmpty();

        RuleFor(c => c.CurrenctCode).NotEmpty();
    }
}