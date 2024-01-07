using FluentValidation;
using SpendingTracker.Application.Handlers.UserSettings.GetUserSettings.Contracts;

namespace SpendingTracker.Application.Handlers.UserSettings.GetUserSettings.Validators;

internal sealed class GetUserSettingsQueryValidator : AbstractValidator<GetUserSettingsQuery>
{
    public GetUserSettingsQueryValidator()
    {
        RuleFor(q => q.UserId).NotEmpty();
        RuleFor(q => q.UserId.Value).NotEmpty();
    }
}