using FluentValidation;
using SpendingTracker.Application.Handlers.Category.UpdateCategory.Contracts;

namespace SpendingTracker.Application.Handlers.Category.UpdateCategory.Validators;

internal class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Title).NotEmpty();
    }
}