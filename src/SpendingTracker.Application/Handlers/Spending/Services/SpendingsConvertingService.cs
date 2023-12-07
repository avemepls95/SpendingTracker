using SpendingTracker.Application.Handlers.Spending.Services.Abstractions;
using SpendingTracker.CurrencyRate.Abstractions;
using SpendingTracker.CurrencyRate.Contracts;

namespace SpendingTracker.Application.Handlers.Spending.Services;

internal class SpendingsConvertingService : ISpendingsConvertingService
{
    private readonly ICurrencyConverter _currencyConverter;

    public SpendingsConvertingService(ICurrencyConverter currencyConverter)
    {
        _currencyConverter = currencyConverter;
    }

    public async Task<Dictionary<Domain.Spending, double>> GetSpendingsConvertedAmountDict(
        Domain.Spending[] spendings,
        Guid targetCurrencyId,
        CancellationToken cancellationToken)
    {
        var convertCurrenciesRequests = spendings.Select(s => new GetCoefficientRequest
        {
            Date = DateOnly.FromDateTime(s.Date.Date),
            CurrencyIdFrom = s.Currency.Id,
            CurrencyIdTo = targetCurrencyId
        }).ToArray();

        var convertCurrenciesResult = await _currencyConverter.GetCoefficients(
            convertCurrenciesRequests,
            cancellationToken);

        var spendingConvertedAmountDict = new Dictionary<Domain.Spending, double>();
        foreach (var spending in spendings)
        {
            var coefficient = convertCurrenciesResult
                .First(item =>
                    item.Date == DateOnly.FromDateTime(spending.Date.Date)
                    && item.CurrencyFrom == spending.Currency.Id)
                .Coefficient;

            var targetCurrencyAmount = spending.Amount * (double) coefficient;
            spendingConvertedAmountDict.Add(spending, targetCurrencyAmount);
        }

        return spendingConvertedAmountDict;
    }
}