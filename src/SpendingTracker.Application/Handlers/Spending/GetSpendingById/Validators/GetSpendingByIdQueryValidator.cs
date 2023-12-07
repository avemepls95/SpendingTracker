using FluentValidation;
using SpendingTracker.Application.Handlers.Spending.GetSpendingById.Contracts;

namespace SpendingTracker.Application.Handlers.Spending.GetSpendingById.Validators;

internal class GetSpendingByIdQueryValidator : AbstractValidator<GetSpendingByIdQuery>
{
    public GetSpendingByIdQueryValidator()
    {
        RuleFor(q => q.Id).NotEmpty();
        RuleFor(q => q.UserId).NotEmpty();
        RuleFor(q => q.UserId.Value).NotEmpty();
    }
}