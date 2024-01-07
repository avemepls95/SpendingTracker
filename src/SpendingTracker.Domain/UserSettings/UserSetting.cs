namespace SpendingTracker.Domain.UserSettings;

public class UserSetting
{
    public UserSetting(Guid id, UserSettingEnum key, string defaultValueAsString)
    {
        Id = id;
        Key = key;
        DefaultValueAsString = defaultValueAsString;
    }

    public Guid Id { get; }
    public UserSettingEnum Key { get; }
    public string DefaultValueAsString { get; }
}