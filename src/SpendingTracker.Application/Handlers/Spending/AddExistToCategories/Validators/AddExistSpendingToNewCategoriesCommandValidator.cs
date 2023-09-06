using FluentValidation;
using SpendingTracker.Application.Handlers.Spending.AddExistToCategories.Contracts;
using SpendingTracker.GenericSubDomain.Validators;

namespace SpendingTracker.Application.Handlers.Spending.AddExistToCategories.Validators;

public class AddExistSpendingToNewCategoriesCommandValidator : AbstractValidator<AddExistSpendingToNewCategoriesCommand>
{
    public AddExistSpendingToNewCategoriesCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.UserId.Value).NotEmpty();

        RuleFor(c => c.SpendingId).NotEmpty();

        RuleForEach(c => c.CategoryTitles).NotEmpty();
        RuleFor(c => c.CategoryTitles).Unique(t => t);
    }
}