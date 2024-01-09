using FluentValidation;
using SpendingTracker.Application.Handlers.Account.UpdateAccount.Contracts;
using SpendingTracker.Domain.Accounts;
using SpendingTracker.GenericSubDomain.Common;

namespace SpendingTracker.Application.Handlers.Account.UpdateAccount.Validators;

internal sealed class CreateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Amount).GreaterThan(0);
        RuleFor(c => c.Name).NotEmpty().MaximumLength(PropertiesMaxLength.AccountName);
        RuleFor(c => c.Type).NotEqual(AccountTypeEnum.None);
        RuleFor(c => c.CurrencyId).NotEmpty();
    }
}