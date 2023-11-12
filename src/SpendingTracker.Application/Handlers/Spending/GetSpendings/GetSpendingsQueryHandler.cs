using SpendingTracker.Application.Handlers.Spending.GetSpendings.Contracts;
using SpendingTracker.Application.Handlers.Spending.GetSpendings.Converters;
using SpendingTracker.CurrencyRate.Abstractions;
using SpendingTracker.CurrencyRate.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Spending.GetSpendings;

internal class GetSpendingsQueryHandler : QueryHandler<GetSpendingsQuery, GetSpendingsResponseItem[]>
{
    private readonly ISpendingRepository _spendingRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrencyConverter _currencyConverter;

    public GetSpendingsQueryHandler(
        ISpendingRepository spendingRepository,
        ICategoryRepository categoryRepository,
        ICurrencyConverter currencyConverter)
    {
        _spendingRepository = spendingRepository;
        _categoryRepository = categoryRepository;
        _currencyConverter = currencyConverter;
    }

    public override async Task<GetSpendingsResponseItem[]> HandleAsync(
        GetSpendingsQuery query,
        CancellationToken cancellationToken)
    {
        var spendings = await _spendingRepository.GetUserSpendings(
            query.UserId,
            query.Offset,
            query.Count,
            cancellationToken);

        var categoriesTree = await _categoryRepository.GetUserCategoriesTree(query.UserId, cancellationToken);

        GetSpendingsResponseItem[] result; 
        if (query.TargetCurrencyId.HasValue)
        {
            var spendingConvertedAmountDict = await GetSpendingConvertedAmountDict(
                spendings,
                query.TargetCurrencyId.Value,
                cancellationToken);

            result = spendings
                .Select(s => SpendingConverter.ConvertToDto(s, spendingConvertedAmountDict[s], categoriesTree))
                .ToArray();
        }
        else
        {
            result = spendings
                .Select(s => SpendingConverter.ConvertToDto(s, categoriesTree))
                .ToArray();
        }

        return result;
    }

    private async Task<Dictionary<Domain.Spending, double>> GetSpendingConvertedAmountDict(
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