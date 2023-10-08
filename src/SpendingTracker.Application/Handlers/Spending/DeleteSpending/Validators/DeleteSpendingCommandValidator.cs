using FluentValidation;
using SpendingTracker.Application.Handlers.Spending.DeleteSpending.Contracts;

namespace SpendingTracker.Application.Handlers.Spending.DeleteSpending.Validators;

internal sealed class DeleteSpendingCommandValidator : AbstractValidator<DeleteSpendingCommand>
{
    public DeleteSpendingCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.ActionSource).NotEmpty();
    }
}