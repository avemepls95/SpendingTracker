using FluentValidation;
using SpendingTracker.Application.Handlers.Spending.GetFilteredSpendings.Contracts;

namespace SpendingTracker.Application.Handlers.Spending.GetFilteredSpendings.Validators;

internal class GetSpendingsInDateRangeQueryValidator : AbstractValidator<GetFilteredSpendingsQuery>
{
    public GetSpendingsInDateRangeQueryValidator()
    {
        RuleFor(q => q.UserId).NotEmpty();
        RuleFor(q => q.UserId.Value).NotEmpty();

        RuleFor(q => q.CategoryId)
            .NotEqual(Guid.Empty)
            .When(q => q.CategoryId.HasValue);

        RuleFor(q => q.DateFrom).NotEmpty();
        RuleFor(q => q.DateTo).NotEmpty();
    }
}