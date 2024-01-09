using SpendingTracker.Application.Handlers.Account.GetAccountsInfo.Contracts;
using SpendingTracker.CurrencyRate.Abstractions;
using SpendingTracker.CurrencyRate.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Account.GetAccountsInfo;

internal sealed class GetAccountsInfoQueryHandler : QueryHandler<GetAccountsInfoQuery, GetAccountsInfoResponse>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrencyConverter _currencyConverter;

    public GetAccountsInfoQueryHandler(IAccountRepository accountRepository, ICurrencyConverter currencyConverter)
    {
        _accountRepository = accountRepository;
        _currencyConverter = currencyConverter;
    }

    public override async Task<GetAccountsInfoResponse> HandleAsync(
        GetAccountsInfoQuery query,
        CancellationToken cancellationToken)
    {
        var userAccounts = await _accountRepository.GetUserAccounts(query.UserId, cancellationToken);

        var now = DateTime.Now;
        var convertCurrenciesRequests = userAccounts.Select(s => new GetCoefficientRequest
        {
            Date = DateOnly.FromDateTime(now),
            CurrencyIdFrom = s.CurrencyId,
            CurrencyIdTo = query.CurrencyId
        }).ToArray();

        var convertCurrenciesResult = await _currencyConverter.GetCoefficients(
            convertCurrenciesRequests,
            cancellationToken);

        var accountDtos = userAccounts.Select(a =>
        {
            var coefficient = convertCurrenciesResult
                .First(item => item.CurrencyFrom == a.CurrencyId)
                .Coefficient;
            var convertedAmount = a.Amount * (double) coefficient;
            return new AccountDto
            {
                Id = a.Id,
                Name = a.Name,
                Type = a.Type,
                OriginalCurrencyId = a.CurrencyId,
                OriginalCurrencyAmount = a.Amount,
                TargetCurrencyAmount = convertedAmount
            };
        }).ToArray();

        var totalTargetCurrencyAmount = accountDtos.Sum(a => a.TargetCurrencyAmount);
        var result = new GetAccountsInfoResponse
        {
            TotalAmount = totalTargetCurrencyAmount,
            Accounts = accountDtos
        };

        return result;
    }
}