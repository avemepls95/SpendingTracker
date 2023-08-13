using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingTracker.Infrastructure.Abstractions.Model;

namespace SpendingTracker.Infrastructure.Configurations
{
    internal sealed class StoredTelegramUserCurrentButtonGroupConfiguration
        : IEntityTypeConfiguration<StoredTelegramUserCurrentButtonGroup>
    {
        public void Configure(EntityTypeBuilder<StoredTelegramUserCurrentButtonGroup> builder)
        {
            builder.ToTable("TelegramUserCurrentButtonGroup");

            builder.HasKey(e => e.UserId);
            builder.Property(i => i.UserId)
                .ValueGeneratedNever();

            builder
                .HasOne<StoredTelegramUser>()
                .WithOne()
                .HasForeignKey<StoredTelegramUserCurrentButtonGroup>(g => g.UserId);

            builder
                .HasIndex(g => new { g.UserId, g.GroupId })
                .IsUnique();
        }
    }
}