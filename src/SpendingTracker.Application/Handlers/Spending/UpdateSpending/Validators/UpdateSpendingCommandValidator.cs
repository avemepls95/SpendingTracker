using FluentValidation;
using SpendingTracker.Application.Handlers.Spending.UpdateSpending.Contracts;
using SpendingTracker.Domain.Constants;

namespace SpendingTracker.Application.Handlers.Spending.UpdateSpending.Validators;

internal class UpdateSpendingCommandValidator : AbstractValidator<UpdateSpendingCommand>
{
    public UpdateSpendingCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Amount).NotEmpty();
        RuleFor(c => c.Date).NotEmpty();
        RuleFor(c => c.CurrencyId).NotEmpty();
        RuleFor(c => c.Description)
            .NotEmpty()
            .MaximumLength(SpendingConstants.DescriptionMaxLength)
            .WithMessage($"Длина описания не должна превышать {SpendingConstants.DescriptionMaxLength} символов");

        RuleFor(c => c.ActionSource).NotEmpty();
    }
}