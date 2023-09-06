using FluentValidation;
using SpendingTracker.Application.Handlers.Category.GetUserCategories.Contracts;

namespace SpendingTracker.Application.Handlers.Category.GetUserCategories.Validators;

internal class GetUserCategoriesQueryValidator : AbstractValidator<GetUserCategoriesQuery>
{
    public GetUserCategoriesQueryValidator()
    {
        RuleFor(q => q.UserId).NotEmpty();
    }
}