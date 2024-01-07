using Microsoft.EntityFrameworkCore;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain.UserSettings;
using SpendingTracker.GenericSubDomain.Validation;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored.UserSettings;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.Infrastructure.Abstractions.Repositories.Models;
using SpendingTracker.Infrastructure.Factories.Abstractions;

namespace SpendingTracker.Infrastructure.Repositories;

internal sealed class UserSettingValuesRepository : IUserSettingValuesRepository
{
    private readonly MainDbContext _dbContext;
    private readonly IUserSettingValueFactory _userSettingValueFactory;

    public UserSettingValuesRepository(MainDbContext dbContext, IUserSettingValueFactory userSettingValueFactory)
    {
        _dbContext = dbContext;
        _userSettingValueFactory = userSettingValueFactory;
    }

    public async Task<UserSettingValue[]> GetValues(UserKey userId, CancellationToken cancellationToken)
    {
        var storedValues = await _dbContext.Set<StoredUserSettingValue>()
            .Include(v => v.Setting)
            .Where(v => v.UserId == userId && !v.Setting.IsDeleted)
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);

        var result = storedValues.Select(_userSettingValueFactory.Create).ToArray();
        return result;
    }

    public async Task CreateOrUpdate(CreateOrUpdateUserSettingValuesModel[] newInputValues, CancellationToken cancellationToken)
    {
        if (newInputValues.Length == 0)
        {
            return;
        }

        var userId = newInputValues.First().UserId;
        if (newInputValues.Any(v => v.UserId != userId))
        {
            throw new Exception("Settings values from different users.");
        }

        if (newInputValues.DistinctBy(v => v.Key).Count() != newInputValues.Length)
        {
            throw new Exception("Collection has non-unique setting keys.");
        }
        
        var settingKeys = newInputValues.Select(v => v.Key).ToArray();
        var dbValues = await _dbContext.Set<StoredUserSettingValue>()
            .Include(v => v.Setting)
            .Where(v => v.UserId == userId && settingKeys.Contains(v.Setting.Key) && !v.Setting.IsDeleted)
            .ToArrayAsync(cancellationToken);

        foreach (var newInputValue in newInputValues)
        {
            var dbValue = dbValues.FirstOrDefault(v => v.Setting.Key == newInputValue.Key);
            if (dbValue is null)
            {
                var setting = await _dbContext.Set<StoredUserSetting>()
                    .Where(s => s.Key == newInputValue.Key && !s.IsDeleted)
                    .FirstOrDefaultAsync(cancellationToken);

                if (setting is null)
                {
                    throw new SpendingTrackerValidationException(ValidationErrorCodeEnum.KeyNotFound);
                }
                
                var newDbValue = new StoredUserSettingValue
                {
                    SettingId = setting.Id,
                    UserId = userId,
                    ValueAsString = newInputValue.NewValueAsString
                };
                await _dbContext.Set<StoredUserSettingValue>().AddAsync(newDbValue, cancellationToken);
            }
            else
            {
                dbValue.ValueAsString = newInputValue.NewValueAsString;
            }
        }
    }
}