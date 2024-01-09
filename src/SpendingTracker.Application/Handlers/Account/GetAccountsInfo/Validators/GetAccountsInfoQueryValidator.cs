using FluentValidation;
using SpendingTracker.Application.Handlers.Account.GetAccountsInfo.Contracts;

namespace SpendingTracker.Application.Handlers.Account.GetAccountsInfo.Validators;

internal sealed class GetAccountsInfoQueryValidator : AbstractValidator<GetAccountsInfoQuery>
{
    public GetAccountsInfoQueryValidator()
    {
        RuleFor(q => q.UserId).NotEmpty();
        RuleFor(q => q.UserId.Value).NotEmpty();

        RuleFor(q => q.CurrencyId).NotEmpty();
    }
}