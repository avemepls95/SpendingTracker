using FluentValidation;
using SpendingTracker.Application.Handlers.Category.CreateCategory.Contracts;

namespace SpendingTracker.Application.Handlers.Category.CreateCategory.Validators;

internal class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(c => c.Title).NotEmpty();
        
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.UserId.Value).NotEmpty();
    }
}