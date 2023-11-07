using FluentValidation;

namespace SpendingTracker.Application.Handlers.Category.AddExistCategoryAsChildren.Validators;

internal class AddExistCategoriesAsChildrenCommandValidator
    : AbstractValidator<Contracts.AddExistCategoryAsChildrenCommand>
{
    public AddExistCategoriesAsChildrenCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.ParentId).NotEmpty();
        RuleFor(c => c.ChildId).NotEmpty();
        RuleFor(c => c.ParentId).NotEqual(c => c.ChildId);
    }
}