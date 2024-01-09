using SpendingTracker.Domain.Accounts;

namespace SpendingTracker.Application.Handlers.Account.GetAccountsInfo.Contracts;

public class AccountDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public AccountTypeEnum Type { get; set; }
    public Guid OriginalCurrencyId { get; set; }
    public double OriginalCurrencyAmount { get; set; }
    public double TargetCurrencyAmount { get; set; }
}