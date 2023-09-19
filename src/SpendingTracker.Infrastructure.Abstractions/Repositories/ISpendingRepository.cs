﻿using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;
using SpendingTracker.Domain.Categories;

namespace SpendingTracker.Infrastructure.Abstractions.Repositories
{
    public interface ISpendingRepository
    {
        Task CreateAsync(Spending spending, CancellationToken cancellationToken = default);

        Task<Spending[]> GetUserSpendingsInRange(
            UserKey userKey,
            DateTimeOffset dateFrom,
            DateTimeOffset dateTo,
            CancellationToken cancellationToken = default);

        Task<Spending[]> GetUserSpendings(
            UserKey userKey,
            int offset,
            int count,
            CancellationToken cancellationToken = default);

        
        Task AddExistToCategories(
            Guid spendingId,
            Category[] categories,
            CancellationToken cancellationToken = default);
    }
}