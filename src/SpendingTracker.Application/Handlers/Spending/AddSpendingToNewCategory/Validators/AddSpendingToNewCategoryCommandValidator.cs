using FluentValidation;
using SpendingTracker.Application.Handlers.Spending.AddSpendingToNewCategory.Contracts;

namespace SpendingTracker.Application.Handlers.Spending.AddSpendingToNewCategory.Validators;

public class AddSpendingToNewCategoryCommandValidator : AbstractValidator<AddSpendingToNewCategoryCommand>
{
    public AddSpendingToNewCategoryCommandValidator()
    {
        RuleFor(c => c.NewCategoryTitle).NotEmpty();
        RuleFor(c => c.SpendingId).NotEmpty();
    }
}