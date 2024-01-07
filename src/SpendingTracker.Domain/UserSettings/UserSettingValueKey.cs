using SpendingTracker.Common.Primitives;

namespace SpendingTracker.Domain.UserSettings
{
    public class UserSettingValueKey : CustomKey<UserSettingValueKey, (UserKey, Guid)>
    {
        public UserSettingValueKey(UserKey userId, Guid settingsId) : base((userId, settingsId))
        {
        }
    }
}