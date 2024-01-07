using FluentValidation;
using SpendingTracker.Application.Handlers.UserSettings.UpdateUserSettings.Contracts;

namespace SpendingTracker.Application.Handlers.UserSettings.UpdateUserSettings.Validators;

internal sealed class UpdateUserSettingsCommandValidator : AbstractValidator<UpdateUserSettingsCommand>
{
    public UpdateUserSettingsCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.UserId.Value).NotEmpty();

        RuleFor(c => c.ViewCurrencyId).NotEmpty();
    }
}