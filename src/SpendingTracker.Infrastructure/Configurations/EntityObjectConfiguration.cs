using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingTracker.Common.Primitives;

namespace SpendingTracker.Infrastructure.Configurations
{
    public class EntityObjectConfiguration<T, TKey> : IEntityTypeConfiguration<T>
        where T : EntityObject<T, TKey>
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(t => t.CreatedBy)
                .HasConversion(v => v.Value, v => new UserKey(v));

            builder.Property(t => t.ModifiedBy)
                .HasConversion(v => v.Value, v => new UserKey(v));
        }
    }
}