using SpendingTracker.Common.Primitives;

namespace SpendingTracker.Domain.UserSettings;

public class UserSettingValue : EntityObject<UserSettingValue, UserSettingValueKey>
{
    public UserSettingValue(Guid settingId, UserKey userId, string valueAsString)
    {
        if (settingId == default)
        {
            throw new ArgumentNullException(nameof(settingId));
        }
        
        if (userId is null || userId.Value == default)
        {
            throw new ArgumentNullException(nameof(userId));
        }
        
        if (string.IsNullOrWhiteSpace(valueAsString))
        {
            throw new ArgumentNullException(nameof(valueAsString));
        }

        SettingId = settingId;
        UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        ValueAsString = valueAsString;
    }

    public UserSettingValue(UserSetting setting, UserKey userId, string valueAsString)
    {
        if (userId is null || userId.Value == default)
        {
            throw new ArgumentNullException(nameof(userId));
        }
        
        if (string.IsNullOrWhiteSpace(valueAsString))
        {
            throw new ArgumentNullException(nameof(valueAsString));
        }

        Setting = setting ?? throw new ArgumentNullException(nameof(setting));
        SettingId = setting.Id;
        UserId = userId;
        ValueAsString = valueAsString;
    }

    public Guid SettingId { get; }
    public UserSetting Setting { get; }
    public UserKey UserId { get; }
    public string ValueAsString { get; }

    public override UserSettingValueKey GetKey()
    {
        return new UserSettingValueKey(UserId, SettingId);
    }
}