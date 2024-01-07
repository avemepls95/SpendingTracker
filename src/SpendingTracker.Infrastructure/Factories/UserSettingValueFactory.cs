using SpendingTracker.Domain.UserSettings;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored.UserSettings;
using SpendingTracker.Infrastructure.Factories.Abstractions;

namespace SpendingTracker.Infrastructure.Factories;

internal class UserSettingValueFactory : IUserSettingValueFactory
{
    private readonly IUserSettingFactory _userSettingFactory;

    public UserSettingValueFactory(IUserSettingFactory userSettingFactory)
    {
        _userSettingFactory = userSettingFactory;
    }

    public UserSettingValue Create(StoredUserSettingValue stored)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (stored.Setting is null)
        {
            return new UserSettingValue(stored.SettingId, stored.UserId, stored.ValueAsString);
        }

        var userSetting = _userSettingFactory.Create(stored.Setting);
        return new UserSettingValue(userSetting, stored.UserId, stored.ValueAsString);
    }
}