using Microsoft.EntityFrameworkCore;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored;
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
        var storedUser = await FindStoredUserByTelegramId(telegramId, cancellationToken);
        if (storedUser is null)
        {
            throw new ArgumentException($"Не найдено пользователя Телеграм с идентификатором {telegramId}");
        }

        var result = _userFactory.Create(storedUser);
        return result;
    }

    public async Task<User> GetById(UserKey id, CancellationToken cancellationToken = default)
    {
        var storedUser = await _dbContext.Set<StoredUser>()
            .Include(u => u.Currency)
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted, cancellationToken);

        if (storedUser is null)
        {
            throw new ArgumentException($"Не найдено пользователя с идентификатором {id}");
        }

        var result = _userFactory.Create(storedUser);
        return result;
    }

    public async Task<User?> FindByTelegramId(long telegramId, CancellationToken cancellationToken)
    {
        var storedUser = await FindStoredUserByTelegramId(telegramId, cancellationToken);
        if (storedUser is null)
        {
            return null;
        }

        var result = _userFactory.Create(storedUser);
        return result;
    }

    public Task<bool> IsExistsById(UserKey id, CancellationToken cancellationToken)
    {
        return _dbContext.Set<StoredUser>().AnyAsync(u => u.Id == id, cancellationToken);
    }

    private async Task<StoredUser?> FindStoredUserByTelegramId(long telegramId, CancellationToken cancellationToken)
    {
        var storedUser = await _dbContext.Set<StoredTelegramUser>()
            .Include(u => u.User)
            .ThenInclude(u => u.Currency)
            .Where(u => u.Id == telegramId)
            .Select(u => u.User)
            .FirstOrDefaultAsync(cancellationToken);

        return storedUser;
    }

    public Task<UserKey?> FindIdByTelegramId(long telegramId, CancellationToken cancellationToken)
    {
        return _dbContext.Set<StoredTelegramUser>()
            .Where(u => u.Id == telegramId)
            .Select(u => u.UserId)
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<UserKey> GetIdByTelegramId(long telegramId, CancellationToken cancellationToken)
    {
        var result = await FindIdByTelegramId(telegramId, cancellationToken);
        if (result is null)
        {
            throw new KeyNotFoundException($"Не найден пользователь с идентификатором Телеграм {telegramId}");
        }
        
        return result;
    }

    public async Task Create(User user, CancellationToken cancellationToken)
    {
        var newUser = new StoredUser(user.Id, user.Currency.Id)
        {
            CurrencyId = user.Currency.Id,
            FirstName = user.FirstName,
            LastName = user.LastName
        };

        await _dbContext.Set<StoredUser>().AddAsync(newUser, cancellationToken);
    }

    public async Task CreateTelegramUser(
        long telegramId,
        string lastName,
        string firstName,
        string userName,
        UserKey userKey,
        CancellationToken cancellationToken)
    {
        var newTelegramUser = new StoredTelegramUser
        {
            Id = telegramId,
            UserId = userKey,
            FirstName = firstName,
            LastName = lastName,
            UserName = userName
        };

        await _dbContext.Set<StoredTelegramUser>().AddAsync(newTelegramUser, cancellationToken);
    }
}