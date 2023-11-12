using FluentValidation;
using SpendingTracker.Application.Handlers.Category.AddNewCategoryAsParent.Contracts;
using SpendingTracker.Application.Handlers.Common;

namespace SpendingTracker.Application.Handlers.Category.AddNewCategoryAsParent.Validators;

internal class AddNewCategoryAsChildCommandValidator : AbstractValidator<AddNewCategoryAsParentCommand>
{
    public AddNewCategoryAsChildCommandValidator()
    {
        RuleFor(c => c.ChildId).NotEmpty();
        RuleFor(c => c.NewParentTitle).NotEmpty();
        RuleFor(c => c.NewParentTitle)
            .MaximumLength(PropertiesMaxLength.CategoryTitle)
            .WithMessage($"Длина названия категории не должна превышать {PropertiesMaxLength.CategoryTitle} символов");

        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.UserId.Value).NotEmpty();
    }
}