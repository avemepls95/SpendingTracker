using SpendingTracker.Domain;

namespace SpendingTracker.Infrastructure.Abstractions.Repositories;

public interface IUserRepository
{
    Task<User> GetByTelegramId(long telegramId, CancellationToken cancellationToken = default);
}