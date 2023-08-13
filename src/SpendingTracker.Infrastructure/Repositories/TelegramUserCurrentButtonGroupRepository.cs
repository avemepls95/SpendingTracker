using Microsoft.EntityFrameworkCore;
using SpendingTracker.Infrastructure.Abstractions.Model;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Infrastructure.Repositories;

internal class TelegramUserCurrentButtonGroupRepository : ITelegramUserCurrentButtonGroupRepository
{
    private readonly MainDbContext _dbContext;

    public TelegramUserCurrentButtonGroupRepository(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> GetIdByUserId(long id, CancellationToken cancellationToken = default)
    {
        var userCurrentButtonGroup = await _dbContext.Set<StoredTelegramUserCurrentButtonGroup>()
            .FirstOrDefaultAsync(r => r.UserId == id, cancellationToken);

        if (userCurrentButtonGroup == null)
        {
            throw new ArgumentException($"Не найдена текущая группа кнопок у пользователя телеграм-идентификатором {id}");
        }

        return userCurrentButtonGroup.GroupId;
    }

    public async Task CreateOrUpdate(long userId, int newGroupId, CancellationToken cancellationToken)
    {
        StoredTelegramUserCurrentButtonGroup group;
        var dbSet = _dbContext.Set<StoredTelegramUserCurrentButtonGroup>();
        if (dbSet.Local.Any(g => g.UserId == userId))
        {
            group = dbSet.Local.First(g => g.UserId == userId);
            group.GroupId = newGroupId;
            _dbContext.Entry(group).State = EntityState.Modified;
        }
        else
        {
            var exist = await _dbContext.Set<StoredTelegramUserCurrentButtonGroup>().AnyAsync(
                g => g.UserId == userId,
                cancellationToken);
            
            group = new StoredTelegramUserCurrentButtonGroup
            {
                UserId = userId,
                GroupId = newGroupId
            };
            
            dbSet.Attach(group);
            _dbContext.Entry(group).State = exist
                ? EntityState.Modified
                : EntityState.Added;
        }
    }
}