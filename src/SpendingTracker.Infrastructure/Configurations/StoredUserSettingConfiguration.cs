using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingTracker.Domain.UserSettings;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored.UserSettings;

namespace SpendingTracker.Infrastructure.Configurations;

internal sealed class StoredUserSettingConfiguration : IEntityTypeConfiguration<StoredUserSetting>
{
    public void Configure(EntityTypeBuilder<StoredUserSetting> builder)
    {
        builder.ToTable("UserSetting");

        builder.HasKey(v => v.Id);

        builder.Property(i => i.Key)
            .IsRequired()
            .ValueGeneratedNever()
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<UserSettingEnum>(v));

        builder.Property(i => i.DefaultValueAsString).IsRequired();
        builder.Property(i => i.IsDeleted).IsRequired();
    }
}