using Microsoft.EntityFrameworkCore;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain.Accounts;
using SpendingTracker.GenericSubDomain.Validation;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.Infrastructure.Abstractions.Repositories.Models;

namespace SpendingTracker.Infrastructure.Repositories;

internal class AccountRepository : IAccountRepository
{
    private readonly MainDbContext _dbContext;

    public AccountRepository(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Create(CreateAccountModel createModel, CancellationToken cancellationToken)
    {
        var newStoredAccount = new StoredAccount
        {
            Id = Guid.NewGuid(),
            UserId = createModel.UserId,
            Type = createModel.Type,
            Name = createModel.Name,
            CurrencyId = createModel.CurrencyId,
            Amount = createModel.Amount
        };

        await _dbContext.Set<StoredAccount>().AddAsync(newStoredAccount, cancellationToken);
    }

    public Task Update(UpdateAccountModel updateModel, CancellationToken cancellationToken)
    {
        return _dbContext.Set<StoredAccount>()
            .Where(a => a.Id == updateModel.Id && !a.IsDeleted)
            .ExecuteUpdateAsync(
                b => b
                    .SetProperty(a => a.Type, updateModel.Type)
                    .SetProperty(a => a.Name, updateModel.Name)
                    .SetProperty(a => a.CurrencyId, updateModel.CurrencyId)
                    .SetProperty(a => a.Amount, updateModel.Amount),
                cancellationToken);
    }

    public async Task<Account[]> GetUserAccounts(UserKey userId, CancellationToken cancellationToken)
    {
        var storedAccounts = await _dbContext.Set<StoredAccount>()
            .Where(a => !a.IsDeleted && a.UserId == userId)
            .ToArrayAsync(cancellationToken);

        if (storedAccounts.Any(s => s.Type == AccountTypeEnum.None))
        {
            throw new Exception("Unexpected value of AccountType None");
        }
        
        return storedAccounts
            .Select(a => new Account(
                a.Id,
                a.UserId,
                a.Type,
                a.Name,
                a.CurrencyId,
                a.Amount,
                a.CreatedDate))
            .ToArray();
    }

    public async Task<Account[]> GetOrderedByDateUserAccounts(UserKey userId, CancellationToken cancellationToken)
    {
        var accounts = await GetUserAccounts(userId, cancellationToken);
        var result = accounts.OrderBy(a => a.CreatedDate).ToArray();
        return result;
    }

    public Task<int> GetUserAccountsCount(UserKey userId, CancellationToken cancellationToken)
    {
        return _dbContext.Set<StoredAccount>()
            .CountAsync(a => !a.IsDeleted && a.UserId == userId, cancellationToken);
    }

    public async Task<Account> GetById(Guid id, CancellationToken cancellationToken)
    {
        var storedAccount = await _dbContext.Set<StoredAccount>().FirstOrDefaultAsync(
            a => a.Id == id && !a.IsDeleted,
            cancellationToken);

        if (storedAccount is null)
        {
            throw new SpendingTrackerValidationException(ValidationErrorCodeEnum.KeyNotFound);
        }

        return new Account(
            storedAccount.Id,
            storedAccount.UserId,
            storedAccount.Type,
            storedAccount.Name,
            storedAccount.CurrencyId,
            storedAccount.Amount,
            storedAccount.CreatedDate);
    }

    public async Task<bool> IsExistsById(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<StoredAccount>().AnyAsync(
            a => a.Id == id && !a.IsDeleted,
            cancellationToken);
    }

    public async Task MarkAsDeleted(Guid id, CancellationToken cancellationToken)
    {
        await _dbContext.Set<StoredAccount>()
            .Where(a => a.Id == id && !a.IsDeleted)
            .ExecuteUpdateAsync(
                b => b
                    .SetProperty(a => a.IsDeleted, true),
                cancellationToken);
    }

    public async Task ChangeAmount(Guid accountId, double delta, CancellationToken cancellationToken)
    {
        var storedAccount = await _dbContext.Set<StoredAccount>().FirstOrDefaultAsync(
            a => a.Id == accountId && !a.IsDeleted,
            cancellationToken);

        if (storedAccount is null)
        {
            throw new SpendingTrackerValidationException(ValidationErrorCodeEnum.KeyNotFound);
        }

        storedAccount.Amount += delta;
    }
}