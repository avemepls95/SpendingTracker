using FluentValidation;
using SpendingTracker.Application.Handlers.UserCurrency.GetUserCurrency.Contracts;

namespace SpendingTracker.Application.Handlers.UserCurrency.GetUserCurrency.Validators;

internal class GetUserCurrencyQueryValidator : AbstractValidator<GetUserCurrencyQuery>
{
    public GetUserCurrencyQueryValidator()
    {
        RuleFor(q => q.UserKey).NotEmpty();
        RuleFor(q => q.UserKey.Value).NotEmpty();
    }
}