using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingTracker.Infrastructure.Abstractions.Models;

namespace SpendingTracker.Infrastructure.Configurations
{
    internal sealed class StoredCurrencyConfiguration : EntityObjectConfiguration<StoredCurrency, Guid>
    {
        public override void Configure(EntityTypeBuilder<StoredCurrency> builder)
        {
            base.Configure(builder);

            builder.ToTable("Currency");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.Property(e => e.Code).HasMaxLength(3);
            builder.HasIndex(e => e.Code).IsUnique();

            builder.Property(e => e.Title).HasMaxLength(100);
            builder.Property(e => e.IsDefault);
        }
    }
}