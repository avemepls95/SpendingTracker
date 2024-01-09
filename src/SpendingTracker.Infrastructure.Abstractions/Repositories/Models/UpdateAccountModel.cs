using SpendingTracker.Domain.Accounts;

namespace SpendingTracker.Infrastructure.Abstractions.Repositories.Models;

public class UpdateAccountModel
{
    public Guid Id { get; init; }
    public AccountTypeEnum Type { get; init; }
    public string Name { get; init; }
    public Guid CurrencyId { get; init; }
    public double Amount { get; init; }
}