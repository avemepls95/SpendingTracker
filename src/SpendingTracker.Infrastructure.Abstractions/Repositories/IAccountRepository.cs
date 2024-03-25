using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain.Accounts;
using SpendingTracker.Infrastructure.Abstractions.Repositories.Models;

namespace SpendingTracker.Infrastructure.Abstractions.Repositories;

public interface IAccountRepository
{
    Task Create(CreateAccountModel createModel, CancellationToken cancellationToken);
    Task Update(UpdateAccountModel updateModel, CancellationToken cancellationToken);
    Task<Account[]> GetUserAccounts(UserKey userId, CancellationToken cancellationToken);
    Task<Account[]> GetOrderedByDateUserAccounts(UserKey userId, CancellationToken cancellationToken);
    Task<int> GetUserAccountsCount(UserKey userId, CancellationToken cancellationToken);
    Task<Account> GetById(Guid id, CancellationToken cancellationToken);
    Task<bool> IsExistsById(Guid id, CancellationToken cancellationToken);
    Task MarkAsDeleted(Guid id, CancellationToken cancellationToken);
    Task ChangeAmount(Guid accountId, double delta, CancellationToken cancellationToken);
}