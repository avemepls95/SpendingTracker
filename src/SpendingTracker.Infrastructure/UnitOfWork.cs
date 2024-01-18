using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Services;

namespace SpendingTracker.Infrastructure;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly MainDbContext _dbContext;
    private readonly ModificationInfoEntityService _modificationInfoService;

    public UnitOfWork(
        MainDbContext dbContext,
        ModificationInfoEntityService modificationInfoService)
    {
        _dbContext = dbContext;
        _modificationInfoService = modificationInfoService;
    }

    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        if (!_dbContext.ChangeTracker.HasChanges())
        {
            return;
        }

        var entries = _dbContext.ChangeTracker.Entries().ToArray();

        await _modificationInfoService.SetModificationInfoAsync(entries, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}