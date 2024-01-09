namespace SpendingTracker.Application.Handlers.Account.GetAccountsInfo.Contracts;

public class GetAccountsInfoResponse
{
    public double TotalAmount { get; set; }
    public AccountDto[] Accounts { get; set; }
}