using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored.Categories;

namespace SpendingTracker.Infrastructure.Configurations;

internal class StoredCategoryConfiguration : EntityObjectConfiguration<StoredCategory, Guid>
{
    public override void Configure(EntityTypeBuilder<StoredCategory> builder)
    {
        base.Configure(builder);

        builder.ToTable("Category");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(i => i.OwnerId)
            .ValueGeneratedNever()
            .HasConversion(v => v.Value, v => new UserKey(v));
        
        builder.Property(e => e.Title).HasMaxLength(100);
        builder.HasMany(c => c.ChildCategoryLinks)
            .WithOne(l => l.Child);
        builder.HasMany(c => c.ParentCategoryLinks)
            .WithOne(l => l.Parent);
    }
}