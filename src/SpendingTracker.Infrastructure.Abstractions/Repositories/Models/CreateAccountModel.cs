using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain.Accounts;

namespace SpendingTracker.Infrastructure.Abstractions.Repositories.Models;

public class CreateAccountModel
{
    public UserKey UserId { get; init; }
    public AccountTypeEnum Type { get; init; }
    public string Name { get; init; }
    public Guid CurrencyId { get; init; }
    public double Amount { get; init; }
}