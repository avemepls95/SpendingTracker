using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingTracker.Infrastructure.Abstractions.Model;

namespace SpendingTracker.Infrastructure.Configurations;

internal sealed class StoredCurrencyRateByDayConfiguration : EntityObjectConfiguration<StoredCurrencyRateByDay, Guid>
{
    public override void Configure(EntityTypeBuilder<StoredCurrencyRateByDay> builder)
    {
        base.Configure(builder);

        builder.ToTable("CurrencyRateByDay");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.Base).IsRequired();
        builder.Property(e => e.Target).IsRequired();
        builder.Property(e => e.Date).IsRequired();
        builder.Property(e => e.Coefficient).IsRequired();
    }
}