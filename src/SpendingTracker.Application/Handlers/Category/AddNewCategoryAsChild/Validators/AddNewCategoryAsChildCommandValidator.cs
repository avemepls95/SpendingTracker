using FluentValidation;
using SpendingTracker.Application.Handlers.Category.AddNewCategoryAsChild.Contracts;

namespace SpendingTracker.Application.Handlers.Category.AddNewCategoryAsChild.Validators;

internal class AddNewCategoryAsChildCommandValidator : AbstractValidator<AddNewCategoryAsChildCommand>
{
    public AddNewCategoryAsChildCommandValidator()
    {
        RuleFor(c => c.ParentId).NotEmpty();
        RuleFor(c => c.ChildTitle).NotEmpty();

        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.UserId.Value).NotEmpty();
    }
}