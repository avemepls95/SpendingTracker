using FluentValidation;
using SpendingTracker.Application.Handlers.Category.AddNewCategoryAsParent.Contracts;

namespace SpendingTracker.Application.Handlers.Category.AddNewCategoryAsParent.Validators;

internal class AddNewCategoryAsChildCommandValidator : AbstractValidator<AddNewCategoryAsParentCommand>
{
    public AddNewCategoryAsChildCommandValidator()
    {
        RuleFor(c => c.ChildId).NotEmpty();
        RuleFor(c => c.NewParentTitle).NotEmpty();

        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.UserId.Value).NotEmpty();
    }
}