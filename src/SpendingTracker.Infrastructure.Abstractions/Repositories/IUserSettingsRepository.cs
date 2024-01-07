using SpendingTracker.Domain.UserSettings;

namespace SpendingTracker.Infrastructure.Abstractions.Repositories;

public interface IUserSettingsRepository
{
    Task<UserSetting[]> GetAll(CancellationToken cancellationToken = default);
}