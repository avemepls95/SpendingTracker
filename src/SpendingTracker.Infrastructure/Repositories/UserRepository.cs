using Microsoft.EntityFrameworkCore;
using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Model;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.Infrastructure.Factories.Abstractions;

namespace SpendingTracker.Infrastructure.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly MainDbContext _dbContext;
    private readonly IUserFactory _userFactory;

    public UserRepository(MainDbContext dbContext, IUserFactory userFactory)
    {
        _dbContext = dbContext;
        _userFactory = userFactory;
    }

    public async Task<User> GetByTelegramId(long telegramId, CancellationToken cancellationToken)
    {
        var storedUser = await _dbContext.Set<StoredTelegramUser>().FirstOrDefaultAsync(
            u => u.Id == telegramId,
            cancellationToken);

        if (storedUser is null)
        {
            throw new ArgumentException($"Не найдено пользователя Телеграм с идентификатором {telegramId}");
        }

        var result = _userFactory.Create(storedUser.UserId);
        return result;
    }
}