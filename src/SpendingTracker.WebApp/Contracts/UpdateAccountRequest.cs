using SpendingTracker.Domain.Accounts;

namespace SpendingTracker.WebApp.Contracts;

public class UpdateAccountRequest
{
    public Guid Id { get; set; }
    public AccountTypeEnum Type { get; set; }
    public string Name { get; set; }
    public Guid CurrencyId { get; set; }
    public double Amount { get; set; }
}