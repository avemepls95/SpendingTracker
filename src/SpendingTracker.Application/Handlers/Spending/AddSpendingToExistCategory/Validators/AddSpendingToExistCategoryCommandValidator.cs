using FluentValidation;
using SpendingTracker.Application.Handlers.Spending.AddSpendingToExistCategory.Contracts;

namespace SpendingTracker.Application.Handlers.Spending.AddSpendingToExistCategory.Validators;

public class AddSpendingToExistCategoryCommandValidator : AbstractValidator<AddSpendingToExistCategoryCommand>
{
    public AddSpendingToExistCategoryCommandValidator()
    {
        RuleFor(c => c.CategoryId).NotEmpty();
        RuleFor(c => c.SpendingId).NotEmpty();
    }
}