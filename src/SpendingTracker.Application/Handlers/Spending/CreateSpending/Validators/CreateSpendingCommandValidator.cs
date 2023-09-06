using FluentValidation;
using SpendingTracker.Application.Handlers.Spending.CreateSpending.Contracts;
using SpendingTracker.Domain.Constants;

namespace SpendingTracker.Application.Handlers.Spending.CreateSpending.Validators;

internal class CreateSpendingCommandValidator : AbstractValidator<CreateSpendingCommand>
{
    public CreateSpendingCommandValidator()
    {
        RuleFor(c => c.Amount).NotEmpty();
        RuleFor(c => c.Date).NotEmpty();
        RuleFor(c => c.Description)
            .NotEmpty()
            .MaximumLength(SpendingConstants.DescriptionMaxLength)
            .WithMessage($"Длина описания не должна превышать {SpendingConstants.DescriptionMaxLength} символов");

        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.UserId.Value).NotEmpty();

        RuleFor(c => c.ActionSource).NotEmpty();
    }
}