using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;

namespace SpendingTracker.Infrastructure.Abstractions.Repositories;

public interface IUserRepository
{
    Task<User> GetByTelegramId(long telegramId, CancellationToken cancellationToken = default);
    Task<User> GetById(UserKey id, CancellationToken cancellationToken = default);
    Task<User?> FindByTelegramId(long telegramId, CancellationToken cancellationToken = default);
    Task<bool> IsExistsTelegramUser(long telegramId, CancellationToken cancellationToken = default);
    Task<UserKey?> FindIdByTelegramId(long telegramId, CancellationToken cancellationToken = default);
    Task<UserKey> GetIdByTelegramId(long telegramId, CancellationToken cancellationToken = default);
    Task Create(User user, CancellationToken cancellationToken = default);
    Task CreateTelegramUser(
        long telegramId,
        string? lastName,
        string firstName,
        string? userName,
        UserKey userKey,
        CancellationToken cancellationToken = default);
}