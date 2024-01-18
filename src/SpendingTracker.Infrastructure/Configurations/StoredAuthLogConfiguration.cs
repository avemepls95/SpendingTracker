using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored;

namespace SpendingTracker.Infrastructure.Configurations
{
    internal sealed class StoredAuthLogConfiguration : IEntityTypeConfiguration<StoredAuthLog>
    {
        public void Configure(EntityTypeBuilder<StoredAuthLog> builder)
        {
            builder.ToTable("AuthLog");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).IsRequired().ValueGeneratedNever();

            builder.Property(e => e.Date).IsRequired();
            builder.Property(e => e.Source).IsRequired();

            builder.Property(i => i.UserId)
                .IsRequired()
                .ValueGeneratedNever()
                .HasConversion(v => v.Value, v => new UserKey(v));
        }
    }
}