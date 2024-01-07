using SpendingTracker.Domain.UserSettings;

namespace SpendingTracker.Infrastructure.Abstractions.Models.Stored.UserSettings;

public class StoredUserSetting
{
    public Guid Id { get; set; }
    public UserSettingEnum Key { get; set; }
    public string DefaultValueAsString { get; }
    public bool IsDeleted { get; set; }
}