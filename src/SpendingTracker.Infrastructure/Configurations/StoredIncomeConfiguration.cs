using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingTracker.Domain.Constants;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored;

namespace SpendingTracker.Infrastructure.Configurations
{
    internal sealed class StoredIncomeConfiguration : EntityObjectConfiguration<StoredIncome, long>
    {
        public override void Configure(EntityTypeBuilder<StoredIncome> builder)
        {
            base.Configure(builder);

            builder.ToTable("Income");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(SpendingConstants.DescriptionMaxLength);

            builder.Property(e => e.Amount)
                .IsRequired();
            
            builder.Property(e => e.Date)
                .IsRequired();
            
            builder.Property(e => e.IsDeleted)
                .IsRequired();
        }
    }
}