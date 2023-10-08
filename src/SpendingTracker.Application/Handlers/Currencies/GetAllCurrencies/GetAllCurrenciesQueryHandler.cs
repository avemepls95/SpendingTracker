using SpendingTracker.Application.Handlers.Currencies.GetAllCurrencies.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Currencies.GetAllCurrencies;

internal sealed class GetAllCurrenciesQueryHandler : QueryHandler<GetAllCurrenciesQuery, GetAllCurrenciesResponseItem[]>
{
    private readonly ICurrencyRepository _currencyRepository;

    public GetAllCurrenciesQueryHandler(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public override async Task<GetAllCurrenciesResponseItem[]> HandleAsync(
        GetAllCurrenciesQuery query,
        CancellationToken cancellationToken)
    {
        var currencies = await _currencyRepository.GetAll(cancellationToken);

        return currencies.Select(c => new GetAllCurrenciesResponseItem
        {
            Id = c.Id,
            Code = c.Code,
            FlagEmojiCode = c.CountryIcon,
            Title = c.Title
        }).ToArray();
    }
}