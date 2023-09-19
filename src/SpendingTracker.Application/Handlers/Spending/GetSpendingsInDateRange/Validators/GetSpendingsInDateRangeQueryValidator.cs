using FluentValidation;
using SpendingTracker.Application.Handlers.Spending.GetSpendingsInDateRange.Contracts;

namespace SpendingTracker.Application.Handlers.Spending.GetSpendingsInDateRange.Validators;

internal class GetSpendingsInDateRangeQueryValidator : AbstractValidator<GetSpendingsInDateRangeQuery>
{
    public GetSpendingsInDateRangeQueryValidator()
    {
        RuleFor(q => q.UserId).NotEmpty();
        RuleFor(q => q.UserId.Value).NotEmpty();

        RuleFor(q => q.DateFrom).NotEmpty();
        RuleFor(q => q.DateTo).NotEmpty();
    }
}