using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain.UserSettings;

namespace SpendingTracker.Infrastructure.Abstractions.Repositories.Models;

public class CreateOrUpdateUserSettingValuesModel
{
    public UserKey UserId { get; set; }
    public UserSettingEnum Key { get; set; }
    public string NewValueAsString { get; set; }
}