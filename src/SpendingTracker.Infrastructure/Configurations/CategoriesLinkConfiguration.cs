using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingTracker.Infrastructure.Abstractions.Model.Categories;

namespace SpendingTracker.Infrastructure.Configurations;

internal class CategoriesLinkConfiguration : IEntityTypeConfiguration<CategoriesLink>
{
    public void Configure(EntityTypeBuilder<CategoriesLink> builder)
    {
        builder.ToTable("CategoriesLink");

        builder.HasKey(l => new { l.ChildId, l.ParentId });
    }
}