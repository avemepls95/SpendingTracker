using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingTracker.Domain;

namespace SpendingTracker.Infrastructure.Configurations
{
    internal sealed class TelegramUserCurrentOperationConfiguration
        : EntityObjectConfiguration<TelegramUserCurrentOperation, Guid>
    {
        public override void Configure(EntityTypeBuilder<TelegramUserCurrentOperation> builder)
        {
            base.Configure(builder);

            builder.ToTable("TelegramUserCurrentOperation");

            builder.HasKey(e => e.Id);
        }
    }
}