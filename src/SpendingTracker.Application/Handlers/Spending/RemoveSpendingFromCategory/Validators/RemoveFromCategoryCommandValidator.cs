using FluentValidation;
using SpendingTracker.Application.Handlers.Spending.RemoveSpendingFromCategory.Contracts;

namespace SpendingTracker.Application.Handlers.Spending.RemoveSpendingFromCategory.Validators;

public class RemoveFromCategoryCommandValidator : AbstractValidator<RemoveSpendingFromCategoryCommand>
{
    public RemoveFromCategoryCommandValidator()
    {
        RuleFor(c => c.CategoryId).NotEmpty();
        RuleFor(c => c.SpendingId).NotEmpty();
    }
}