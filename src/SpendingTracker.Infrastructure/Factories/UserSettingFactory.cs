using SpendingTracker.Domain.UserSettings;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored.UserSettings;
using SpendingTracker.Infrastructure.Factories.Abstractions;

namespace SpendingTracker.Infrastructure.Factories;

internal class UserSettingFactory : IUserSettingFactory
{
    public UserSetting Create(StoredUserSetting stored)
    {
        return new UserSetting(stored.Id, stored.Key, stored.DefaultValueAsString);
    }
}