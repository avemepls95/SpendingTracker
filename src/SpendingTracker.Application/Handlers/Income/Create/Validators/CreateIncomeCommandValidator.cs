using FluentValidation;
using SpendingTracker.Application.Handlers.Income.Create.Contracts;
using SpendingTracker.Domain.Constants;

namespace SpendingTracker.Application.Handlers.Income.Create.Validators;

internal sealed class CreateIncomeCommandValidator : AbstractValidator<CreateIncomeCommand>
{
    public CreateIncomeCommandValidator()
    {
        RuleFor(c => c.Amount).NotEmpty();
        RuleFor(c => c.Date).NotEmpty();
        RuleFor(c => c.Description)
            .NotEmpty()
            .MaximumLength(IncomeConstants.DescriptionMaxLength)
            .WithMessage($"Длина описания не должна превышать {IncomeConstants.DescriptionMaxLength} символов");

        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.UserId.Value).NotEmpty();

        RuleFor(c => c.AccountId).NotEmpty();
    }    
}