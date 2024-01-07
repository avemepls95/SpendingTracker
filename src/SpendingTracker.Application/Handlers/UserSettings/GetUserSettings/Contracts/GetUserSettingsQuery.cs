using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.UserSettings.GetUserSettings.Contracts;

public class GetUserSettingsQuery : IQuery<GetUserSettingsQueryResponse>
{
    public UserKey UserId { get; set; }
}