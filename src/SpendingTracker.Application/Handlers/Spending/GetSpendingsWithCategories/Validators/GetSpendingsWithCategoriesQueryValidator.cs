using FluentValidation;
using SpendingTracker.Application.Handlers.Spending.GetSpendingsWithCategories.Contracts;

namespace SpendingTracker.Application.Handlers.Spending.GetSpendingsWithCategories.Validators;

internal class GetSpendingsWithCategoriesQueryValidator : AbstractValidator<GetSpendingsWithCategoriesQuery>
{
    public GetSpendingsWithCategoriesQueryValidator()
    {
        RuleFor(q => q.UserId).NotEmpty();
        RuleFor(q => q.UserId.Value).NotEmpty();
        
        RuleFor(q => q.Offset).Must(o => o >= 0);
        RuleFor(q => q.Count).NotEmpty();
    }
}