namespace SpendingTracker.Infrastructure.Abstractions.Repositories;

public interface ITelegramUserCurrentButtonGroupRepository
{
    Task<int> GetIdByUserId(long id, CancellationToken cancellationToken = default);
    Task CreateOrUpdate(long userId, int newGroupId, CancellationToken cancellationToken = default);
}