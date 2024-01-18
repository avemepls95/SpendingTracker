using FluentValidation;
using SpendingTracker.Application.Handlers.Auth.CreateAuthLog.Contracts;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored;

namespace SpendingTracker.Application.Handlers.Auth.CreateAuthLog.Validators;

internal sealed class CreateAuthLogCommandValidator : AbstractValidator<CreateAuthLogCommand>
{
    public CreateAuthLogCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.UserId.Value).NotEmpty();
        RuleFor(c => c.Source).NotEqual(AuthSource.None);
    }
}