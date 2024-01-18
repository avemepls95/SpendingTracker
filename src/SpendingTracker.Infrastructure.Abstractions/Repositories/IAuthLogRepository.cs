using SpendingTracker.Common.Primitives;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored;

namespace SpendingTracker.Infrastructure.Abstractions.Repositories;

public interface IAuthLogRepository
{
    Task Create(UserKey userId, AuthSource source, object? additionalData, CancellationToken cancellationToken);
}