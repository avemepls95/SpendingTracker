using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;

namespace SpendingTracker.Infrastructure.Factories.Abstractions;

public interface IUserFactory
{
    User Create(UserKey userId);
}