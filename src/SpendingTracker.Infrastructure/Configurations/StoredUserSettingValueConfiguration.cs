using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain.UserSettings;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored.UserSettings;

namespace SpendingTracker.Infrastructure.Configurations;

internal sealed class StoredUserSettingValueConfiguration
    : EntityObjectConfiguration<StoredUserSettingValue, UserSettingValueKey>
{
    public override void Configure(EntityTypeBuilder<StoredUserSettingValue> builder)
    {
        base.Configure(builder);

        builder.ToTable("UserSettingValue");

        builder.HasKey(v => new { v.SettingId, v.UserId });

        builder.Property(i => i.SettingId).IsRequired();
        builder.Property(i => i.ValueAsString).IsRequired();

        builder.Property(i => i.UserId)
            .IsRequired()
            .ValueGeneratedNever()
            .HasConversion(v => v.Value, v => new UserKey(v));
    }
}