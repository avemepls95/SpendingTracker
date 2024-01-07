using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain.UserSettings;
using SpendingTracker.Infrastructure.Abstractions.Repositories.Models;

namespace SpendingTracker.Infrastructure.Abstractions.Repositories;

public interface IUserSettingValuesRepository
{
    Task<UserSettingValue[]> GetValues(UserKey userId, CancellationToken cancellationToken = default);
    Task CreateOrUpdate(CreateOrUpdateUserSettingValuesModel[] newInputValues, CancellationToken cancellationToken);
}