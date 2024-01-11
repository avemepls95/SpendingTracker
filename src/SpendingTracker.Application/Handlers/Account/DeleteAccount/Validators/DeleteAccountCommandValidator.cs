using FluentValidation;
using SpendingTracker.Application.Handlers.Account.DeleteAccount.Contracts;

namespace SpendingTracker.Application.Handlers.Account.DeleteAccount.Validators;

internal sealed class CreateAccountCommandValidator : AbstractValidator<DeleteAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}