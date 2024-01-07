using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.UserSettings.UpdateUserSettings.Contracts;

public class UpdateUserSettingsCommand : ICommand
{
    public UserKey UserId { get; init; }
    public Guid ViewCurrencyId { get; init; }
}