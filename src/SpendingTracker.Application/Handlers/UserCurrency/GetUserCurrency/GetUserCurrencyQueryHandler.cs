using SpendingTracker.Application.Handlers.UserCurrency.GetUserCurrency.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.UserCurrency.GetUserCurrency;

internal class GetUserCurrencyQueryHandler : QueryHandler<GetUserCurrencyQuery, Currency>
{
    private readonly IUserCurrencyRepository _userCurrencyRepository;

    public GetUserCurrencyQueryHandler(IUserCurrencyRepository userCurrencyRepository)
    {
        _userCurrencyRepository = userCurrencyRepository;
    }

    public override Task<Currency> HandleAsync(GetUserCurrencyQuery query, CancellationToken cancellationToken)
    {
        return _userCurrencyRepository.Get(query.UserKey, cancellationToken);
    }
}