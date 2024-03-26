using Microsoft.Extensions.DependencyInjection;
using SpendingTracker.CurrencyRate.Abstractions;
using SpendingTracker.Domain.Constants;
using SpendingTracker.GenericSubDomain.StartupTasks.Abstractions;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.CurrencyRate.StartupTasks
{
    internal class ActualizeCurrencyRatesByDaysStartupTask : IStartupTask
    {
        private readonly IServiceProvider _provider;

        public ActualizeCurrencyRatesByDaysStartupTask(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task Execute()
        {
            using var scope = _provider.CreateScope();
            var currencyRateByDayRepository = scope.ServiceProvider.GetService<ICurrencyRateByDayRepository>()!;
            var ratesProvider = scope.ServiceProvider.GetService<IRatesProvider>()!;
            var currencyRepository = scope.ServiceProvider.GetService<ICurrencyRepository>()!;
            var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>()!;
            
            var allCurrencies = await currencyRepository.GetAll();
            var targetCurrencies = allCurrencies.Where(c => !c.IsDefault).ToArray();

            foreach (var targetCurrency in targetCurrencies)
            {
                var missedDaysByCurrency = await currencyRateByDayRepository.GetMissedDaysFromDate(
                    SpendingConstants.MinDate.Date,
                    targetCurrency.Id);

                if (missedDaysByCurrency.Length == 0)
                {
                    continue;
                }
            
                var missedDaysRates = await ratesProvider.GetByDates(
                    CurrencyOptions.BaseCurrencyCode,
                    new []{ targetCurrency.Code },
                    missedDaysByCurrency);
            
                foreach (var rateInfo in missedDaysRates)
                {
                    await currencyRateByDayRepository.Create(
                        rateInfo.Date.ToDateTimeOffset(TimeZoneInfo.Utc),
                        targetCurrency.Id,
                        rateInfo.Coefficient);
                }

                await unitOfWork.SaveAsync();
            }
        }
    }
}
