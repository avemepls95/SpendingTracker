using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain.Accounts;
using SpendingTracker.GenericSubDomain.Common;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored;

namespace SpendingTracker.Infrastructure.Configurations
{
    internal sealed class StoredAccountConfiguration : EntityObjectConfiguration<StoredAccount, Guid>
    {
        public override void Configure(EntityTypeBuilder<StoredAccount> builder)
        {
            base.Configure(builder);

            builder.ToTable("Account");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            
            builder.Property(e => e.UserId)
                .IsRequired()
                .HasConversion(v => v.Value, v => new UserKey(v));

            builder.Property(e => e.Name).IsRequired().HasMaxLength(PropertiesMaxLength.AccountName);
            builder.Property(e => e.CurrencyId).IsRequired();
            builder.Property(e => e.Amount).IsRequired();
            builder.Property(e => e.IsDeleted).IsRequired();
            
            builder.Property(i => i.Type)
                .IsRequired()
                .HasConversion(v => v.ToString(), v => Enum.Parse<AccountTypeEnum>(v));
        }
    }
}