using FluentValidation;
using SpendingTracker.Application.Handlers.Category.CreateCategory.Contracts;
using SpendingTracker.GenericSubDomain.Common;

namespace SpendingTracker.Application.Handlers.Category.CreateCategory.Validators;

internal class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(c => c.Title).NotEmpty();
        RuleFor(c => c.Title)
            .MaximumLength(PropertiesMaxLength.CategoryTitle)
            .WithMessage($"Длина названия категории не должна превышать {PropertiesMaxLength.CategoryTitle} символов");
        
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.UserId.Value).NotEmpty();
    }
}