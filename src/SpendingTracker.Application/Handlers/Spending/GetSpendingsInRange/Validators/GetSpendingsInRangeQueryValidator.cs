using FluentValidation;
using SpendingTracker.Application.Handlers.Spending.GetSpendingsInRange.Contracts;

namespace SpendingTracker.Application.Handlers.Spending.GetSpendingsInRange.Validators;

internal class GetSpendingsInRangeQueryValidator : AbstractValidator<GetSpendingsInRangeQuery>
{
    public GetSpendingsInRangeQueryValidator()
    {
        RuleFor(q => q.UserId).NotEmpty();
        RuleFor(q => q.UserId.Value).NotEmpty();

        RuleFor(q => q.DateFrom).NotEmpty();
        RuleFor(q => q.DateTo).NotEmpty();
    }
}