using Microsoft.EntityFrameworkCore;
using SpendingTracker.Domain.UserSettings;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored.UserSettings;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.Infrastructure.Factories.Abstractions;

namespace SpendingTracker.Infrastructure.Repositories;

internal class UserSettingsRepository : IUserSettingsRepository
{
    private readonly IUserSettingFactory _userSettingFactory;
    private readonly MainDbContext _dbContext;

    public UserSettingsRepository(IUserSettingFactory userSettingFactory, MainDbContext dbContext)
    {
        _userSettingFactory = userSettingFactory;
        _dbContext = dbContext;
    }

    public async Task<UserSetting[]> GetAll(CancellationToken cancellationToken)
    {
        var storedSettings = await _dbContext.Set<StoredUserSetting>()
            .Where(s => !s.IsDeleted)
            .ToArrayAsync(cancellationToken);

        var result = storedSettings.Select(_userSettingFactory.Create).ToArray();
        return result;
    }
}