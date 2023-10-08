using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored.Categories;

namespace SpendingTracker.Infrastructure.Configurations;

internal class SpendingCategoryLinkConfiguration : IEntityTypeConfiguration<SpendingCategoryLink>
{
    public void Configure(EntityTypeBuilder<SpendingCategoryLink> builder)
    {
        builder.ToTable("SpendingCategoryLink");
        
        builder.HasKey(l => new { l.SpendingId, l.CategoryId });
    }
}