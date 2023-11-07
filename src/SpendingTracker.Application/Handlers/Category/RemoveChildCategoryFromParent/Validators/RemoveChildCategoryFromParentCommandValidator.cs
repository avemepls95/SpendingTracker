using FluentValidation;
using SpendingTracker.Application.Handlers.Category.RemoveChildCategoryFromParent.Contracts;

namespace SpendingTracker.Application.Handlers.Category.RemoveChildCategoryFromParent.Validators;

internal class RemoveChildCategoryFromParentCommandValidator : AbstractValidator<RemoveChildCategoryFromParentCommand>
{
    public RemoveChildCategoryFromParentCommandValidator()
    {
        RuleFor(c => c.ChildId).NotEmpty();
        RuleFor(c => c.ParentId).NotEmpty();
    }
}