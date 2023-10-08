using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Infrastructure.Abstractions.Models;

namespace SpendingTracker.Infrastructure.Configurations
{
    internal sealed class StoredTelegramUserConfiguration : EntityObjectConfiguration<StoredTelegramUser, long>
    {
        public override void Configure(EntityTypeBuilder<StoredTelegramUser> builder)
        {
            base.Configure(builder);

            builder.ToTable("TelegramUser");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            
            builder.Property(i => i.UserId)
                .ValueGeneratedNever()
                .HasConversion(v => v.Value, v => new UserKey(v));
        }
    }
}