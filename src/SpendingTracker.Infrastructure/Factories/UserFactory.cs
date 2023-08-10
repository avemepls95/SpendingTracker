using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Factories.Abstractions;

namespace SpendingTracker.Infrastructure.Factories;

internal class UserFactory : IUserFactory
{
    public User Create(UserKey userId)
    {
        return new User(userId);
    }
}