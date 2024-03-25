using FluentValidation;
using SpendingTracker.Application.Handlers.Income.DeleteLast.Contracts;

namespace SpendingTracker.Application.Handlers.Income.DeleteLast.Validators;

internal sealed class DeleteLastIncomeCommandValidator : AbstractValidator<DeleteLastIncomeCommand>
{
    public DeleteLastIncomeCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.ActionSource).NotEmpty();
    }
}