using FluentValidation;
using SpendingTracker.Application.Handlers.Category.DeleteCategory.Contracts;

namespace SpendingTracker.Application.Handlers.Category.DeleteCategory.Validators;

internal class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        
        RuleFor(c => c.InitiatorId).NotEmpty();
        RuleFor(c => c.InitiatorId.Value).NotEmpty();
    }
}