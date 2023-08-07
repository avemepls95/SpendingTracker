using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingTracker.Domain;

namespace SpendingTracker.Infrastructure.Configurations
{
    internal sealed class SpendingConfiguration : EntityObjectConfiguration<Spending, Guid>
    {
        public override void Configure(EntityTypeBuilder<Spending> builder)
        {
            base.Configure(builder);

            builder.ToTable("Spending");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
        }
    }
}