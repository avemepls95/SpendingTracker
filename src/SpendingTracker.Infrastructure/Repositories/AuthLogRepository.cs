using Newtonsoft.Json;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Infrastructure.Repositories;

internal sealed class AuthLogRepository : IAuthLogRepository
{
    private readonly MainDbContext _dbContext;

    public AuthLogRepository(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Create(UserKey userId, AuthSource source, object? additionalData, CancellationToken cancellationToken)
    {
        var newLog = new StoredAuthLog
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Date = DateTimeOffset.UtcNow,
            Source = source,
            AdditionalData = additionalData is null
                ? null
                : JsonConvert.SerializeObject(additionalData)
        };

        await _dbContext.Set<StoredAuthLog>().AddAsync(newLog, cancellationToken);
    }
}