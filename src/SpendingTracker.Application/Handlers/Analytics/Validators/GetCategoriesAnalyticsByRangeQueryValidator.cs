using FluentValidation;
using SpendingTracker.Application.Handlers.Analytics.GetCategoriesAnalyticsByRange.Contracts;

namespace SpendingTracker.Application.Handlers.Analytics.Validators;

internal sealed class GetCategoriesAnalyticsByRangeQueryValidator : AbstractValidator<GetCategoriesAnalyticsByRangeQuery>
{
    public GetCategoriesAnalyticsByRangeQueryValidator()
    {
        RuleFor(q => q.UserId).NotEmpty();
        RuleFor(q => q.UserId.Value).NotEmpty();

        RuleFor(q => q.TargetCurrencyId).NotEmpty();
        RuleFor(q => q.DateFrom).NotEmpty();
        RuleFor(q => q.DateTo).NotEmpty();
        RuleFor(q => q).Must(q => q.DateFrom < q.DateTo);
    }
}