using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain.UserSettings;

namespace SpendingTracker.Infrastructure.Abstractions.Models.Stored.UserSettings;

public class StoredUserSettingValue : EntityObject<StoredUserSettingValue, UserSettingValueKey>
{
    public Guid SettingId { get; set; }
    public StoredUserSetting Setting { get; set; }
    public UserKey UserId { get; set; }
    public string ValueAsString { get; set; }

    public override UserSettingValueKey GetKey()
    {
        return new UserSettingValueKey(UserId, SettingId);
    }
}