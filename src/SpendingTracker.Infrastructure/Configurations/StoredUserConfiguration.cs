using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Infrastructure.Abstractions.Models;

namespace SpendingTracker.Infrastructure.Configurations
{
    internal sealed class StoredUserConfiguration : EntityObjectConfiguration<StoredUser, UserKey>
    {
        public override void Configure(EntityTypeBuilder<StoredUser> builder)
        {
            base.Configure(builder);

            builder.ToTable("User");

            builder.HasKey(e => e.Id);
            builder.Property(i => i.Id)
                .ValueGeneratedNever()
                .HasConversion(v => v.Value, v => new UserKey(v));
        }
    }
}