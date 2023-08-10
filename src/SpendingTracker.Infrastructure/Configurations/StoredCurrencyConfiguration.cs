using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Model;

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
        }
    }
}