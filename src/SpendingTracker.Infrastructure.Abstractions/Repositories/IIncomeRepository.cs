using SpendingTracker.Common.Primitives;
using SpendingTracker.Infrastructure.Abstractions.Models.Request;

namespace SpendingTracker.Infrastructure.Abstractions.Repositories;

public interface IIncomeRepository
{
    Task Create(CreateIncomeRequest request, CancellationToken cancellationToken);
    Task DeleteLast(UserKey userId, CancellationToken cancellationToken);
}