using FluentValidation;
using SpendingTracker.Application.Handlers.Spending.GetSpendings.Contracts;

namespace SpendingTracker.Application.Handlers.Spending.GetSpendings.Validators;

internal class GetSpendingsQueryValidator : AbstractValidator<GetSpendingsQuery>
{
    public GetSpendingsQueryValidator()
    {
        RuleFor(q => q.UserId).NotEmpty();
        RuleFor(q => q.UserId.Value).NotEmpty();

        RuleFor(q => q.Offset).NotEmpty();
        RuleFor(q => q.Count).NotEmpty();
    }
}