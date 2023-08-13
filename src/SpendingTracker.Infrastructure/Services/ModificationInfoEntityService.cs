using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SpendingTracker.Common.Primitives;
using SpendingTracker.GenericSubDomain.User;
using SpendingTracker.GenericSubDomain.User.Abstractions;
using SpendingTracker.GenericSubDomain.User.Internal;

namespace SpendingTracker.Infrastructure.Services
{
    internal sealed class ModificationInfoEntityService
    {
        private readonly IUserContextFactoryProvider _userContextFactoryProvider;

        public ModificationInfoEntityService(
            IUserContextFactoryProvider userContextFactoryProvider)
        {
            _userContextFactoryProvider = userContextFactoryProvider;
        }

        private async Task<UserKey> GetCurrentUserIdAsync(CancellationToken cancellationToken)
        {
            var userContextFactory = _userContextFactoryProvider.GetSuitableFactory();

            var userContext = await userContextFactory.CreateUserContextAsync(cancellationToken);
            return userContext.UserId;
        }

        public async Task SetModificationInfoAsync(
            IEnumerable<EntityEntry> entries,
            CancellationToken cancellationToken)
        {
            var targetEntries = entries
                .Where(e => e.Entity is IModificationInfoAccessor)
                .ToArray();

            if (!targetEntries.Any())
                return;

            var now = DateTimeOffset.UtcNow;
            var userId = await GetCurrentUserIdAsync(cancellationToken);

            foreach (var entry in targetEntries)
            {
                var entity = (IModificationInfoAccessor)entry.Entity;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.SetCreated(now, userId);
                        break;

                    case EntityState.Modified:
                        entity.SetModified(now, userId);
                        break;
                }
            }
        }
    }
}