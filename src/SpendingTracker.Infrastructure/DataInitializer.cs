using Microsoft.EntityFrameworkCore;
using SpendingTracker.Infrastructure.Abstractions;

namespace SpendingTracker.Infrastructure;

internal class DataInitializer : IDataInitializer
{
    private readonly MainDbContext _dbContext;

    public DataInitializer(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Initialize()
    {
        _dbContext.Database.Migrate();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}