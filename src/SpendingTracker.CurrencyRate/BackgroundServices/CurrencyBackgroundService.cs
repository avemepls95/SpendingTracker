using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpendingTracker.CurrencyRate.Abstractions;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.CurrencyRate.BackgroundServices;

internal class CurrencyBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IRatesProvider _ratesProvider;

    public CurrencyBackgroundService(IServiceScopeFactory serviceScopeFactory, IRatesProvider ratesProvider)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _ratesProvider = ratesProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var now= DateTimeOffset.UtcNow;
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var currencyRateByDayRepository = scope.ServiceProvider.GetService<ICurrencyRateByDayRepository>()!;
                var currencyRepository = scope.ServiceProvider.GetService<ICurrencyRepository>()!;
                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>()!;

                if (await currencyRateByDayRepository.ExistsAnyInThisDay(now, cancellationToken))
                {
                    await DelayToNextDay(now, cancellationToken);
                }

                var allCurrencies = await currencyRepository.GetAll(cancellationToken);
                var targetCurrencies = allCurrencies.Where(c => !c.IsDefault).ToArray();
                var targetCurrencyCodes = targetCurrencies.Select(c => c.Code).ToArray();
                var rates = await _ratesProvider.Get(
                    CurrencyOptions.BaseCurrencyCode,
                    targetCurrencyCodes,
                    cancellationToken);

                var currencyCodeIdDict = targetCurrencies.ToDictionary(c => c.Code, c => c.Id);
                foreach (var rateInfo in rates)
                {
                    var targetCurrency = currencyCodeIdDict[rateInfo.TargetCode];
                    await currencyRateByDayRepository.Create(
                        DateTimeOffset.UtcNow,
                        targetCurrency,
                        rateInfo.Coefficient,
                        cancellationToken);
                }

                await unitOfWork.SaveAsync(cancellationToken);
            }
            catch (Exception e)
            {
                // TODO: логирование
            }

            await DelayToNextDay(now, cancellationToken);
        }
    }

    private Task DelayToNextDay(DateTimeOffset now, CancellationToken cancellationToken)
    {
        var remainingFromNextDayTimeSpan = TimeSpan.FromHours(24) - now.TimeOfDay;
        var delayTimeSpan = remainingFromNextDayTimeSpan.Add(TimeSpan.FromMinutes(10));
        return Task.Delay(delayTimeSpan, cancellationToken);
    }
}