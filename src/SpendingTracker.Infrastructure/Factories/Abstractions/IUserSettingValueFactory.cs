using SpendingTracker.Domain.UserSettings;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored.UserSettings;

namespace SpendingTracker.Infrastructure.Factories.Abstractions;

internal interface IUserSettingValueFactory
{
    UserSettingValue Create(StoredUserSettingValue stored);
}