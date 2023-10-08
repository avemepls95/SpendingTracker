using FluentValidation;
using SpendingTracker.Application.Handlers.Spending.DeleteLastSpending.Contracts;

namespace SpendingTracker.Application.Handlers.Spending.DeleteLastSpending.Validators;

internal sealed class DeleteLastSpendingCommandValidator : AbstractValidator<DeleteLastSpendingCommand>
{
    public DeleteLastSpendingCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.ActionSource).NotEmpty();
    }
}