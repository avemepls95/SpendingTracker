using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SpendingTracker.Common.Primitives;
using SpendingTracker.GenericSubDomain.User;

namespace SpendingTracker.Infrastructure.Services
{
    internal sealed class ModificationInfoEntityService
    {
        private readonly UserContextFactoryProvider _userContextFactoryProvider;

        public ModificationInfoEntityService(
            UserContextFactoryProvider userContextFactoryProvider)
        {
            _userContextFactoryProvider = userContextFactoryProvider;
        }

        private async Task<UserKey> GetCurrentUserIdAsync(CancellationToken cancellationToken)
        {
            var userContextFactory = _userContextFactoryProvider.GetSuitableFactory();

            var userContext = await userContextFactory.CreateUserContextAsync(cancellationToken);
            return userContext.CurrentUser.Id;
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

            var now = DateTimeOffset.Now;
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