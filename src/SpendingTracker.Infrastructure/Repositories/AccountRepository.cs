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
            .Select(a => new Account
            {
                Id = a.Id,
                UserId = userId,
                Type = a.Type,
                Name = a.Name,
                CurrencyId = a.CurrencyId,
                Amount = a.Amount,
                IsDeleted = a.IsDeleted
            })
            .ToArray();
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

        return new Account
        {
            Id = storedAccount.Id,
            UserId = storedAccount.UserId,
            Type = storedAccount.Type,
            Name = storedAccount.Name,
            CurrencyId = storedAccount.CurrencyId,
            Amount = storedAccount.Amount,
            IsDeleted = storedAccount.IsDeleted
        };
    }

    public Task<bool> IsExistsById(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Set<StoredAccount>().AnyAsync(
            a => a.Id == id && !a.IsDeleted,
            cancellationToken);
    }
}