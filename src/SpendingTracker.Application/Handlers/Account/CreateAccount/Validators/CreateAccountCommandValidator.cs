using FluentValidation;
using SpendingTracker.Application.Handlers.Account.CreateAccount.Contracts;
using SpendingTracker.Domain.Accounts;
using SpendingTracker.GenericSubDomain.Common;

namespace SpendingTracker.Application.Handlers.Account.CreateAccount.Validators;

internal sealed class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.UserId.Value).NotEmpty();

        RuleFor(c => c.Amount).GreaterThan(0);
        RuleFor(c => c.Name).NotEmpty().MaximumLength(PropertiesMaxLength.AccountName);
        RuleFor(c => c.Type).NotEqual(AccountTypeEnum.None);
        RuleFor(c => c.CurrencyId).NotEmpty();
    }
}