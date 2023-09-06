using FluentValidation;
using SpendingTracker.GenericSubDomain.Validators;

namespace SpendingTracker.Application.Handlers.Category.AddExistCategoriesAsChildrenCommand.Validators;

internal class AddExistCategoriesAsChildrenCommandValidator
    : AbstractValidator<Contracts.AddExistCategoriesAsChildrenCommand>
{
    public AddExistCategoriesAsChildrenCommandValidator()
    {
        RuleFor(c => c.ParentId).NotEmpty();
        RuleFor(c => c.ChildIds)
            .NotEmpty()
            .Unique(id => id);
    }
}