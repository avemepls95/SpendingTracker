using Microsoft.EntityFrameworkCore;
using SpendingTracker.Infrastructure.Configurations;

namespace SpendingTracker.Infrastructure
{
    public sealed class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new StoredSpendingConfiguration())
                .ApplyConfiguration(new StoredCurrencyConfiguration())
                .ApplyConfiguration(new StoredUserConfiguration())
                .ApplyConfiguration(new StoredTelegramUserConfiguration())
                .ApplyConfiguration(new StoredTelegramUserCurrentButtonGroupConfiguration());
        }
    }
}