using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored.Categories;

namespace SpendingTracker.Infrastructure.Configurations;

internal class CategoriesLinkConfiguration : IEntityTypeConfiguration<StoredCategoriesLink>
{
    public void Configure(EntityTypeBuilder<StoredCategoriesLink> builder)
    {
        builder.ToTable("CategoriesLink");

        builder.HasKey(l => new { l.ChildId, l.ParentId });
    }
}