using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingTracker.Domain;

namespace SpendingTracker.Infrastructure.Configurations
{
    internal sealed class CurrencyConfiguration : EntityObjectConfiguration<Currency, Guid>
    {
        public override void Configure(EntityTypeBuilder<Currency> builder)
        {
            base.Configure(builder);

            builder.ToTable("Currency");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
        }
    }
}