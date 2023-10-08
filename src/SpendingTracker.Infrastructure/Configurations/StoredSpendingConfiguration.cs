using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingTracker.Domain.Constants;
using SpendingTracker.Infrastructure.Abstractions.Models;

namespace SpendingTracker.Infrastructure.Configurations
{
    internal sealed class StoredSpendingConfiguration : EntityObjectConfiguration<StoredSpending, Guid>
    {
        public override void Configure(EntityTypeBuilder<StoredSpending> builder)
        {
            base.Configure(builder);

            builder.ToTable("Spending");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.Property(e => e.Description).HasMaxLength(SpendingConstants.DescriptionMaxLength);
        }
    }
}