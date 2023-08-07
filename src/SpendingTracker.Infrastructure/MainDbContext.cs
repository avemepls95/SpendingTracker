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
                .ApplyConfiguration(new SpendingConfiguration());
        }
    }
}