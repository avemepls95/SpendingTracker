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
            
            var missedDays = await currencyRateByDayRepository.GetMissedDaysFromDate(SpendingConstants.MinDate.Date);
            if (missedDays.Length == 0)
            {
                return;
            }
            
            var allCurrencies = await currencyRepository.GetAll();
            var targetCurrencies = allCurrencies.Where(c => !c.IsDefault).ToArray();
            var targetCurrencyCodes = targetCurrencies.Select(c => c.Code).ToArray();
            var missedDaysRates = await ratesProvider.GetByDates(
                CurrencyOptions.BaseCurrencyCode,
                targetCurrencyCodes,
                missedDays);
            
            var currencyCodeIdDict = targetCurrencies.ToDictionary(c => c.Code, c => c.Id);
            foreach (var rateInfo in missedDaysRates)
            {
                var targetCurrency = currencyCodeIdDict[rateInfo.TargetCode];
                await currencyRateByDayRepository.Create(
                    rateInfo.Date.ToDateTimeOffset(TimeZoneInfo.Utc),
                    targetCurrency,
                    rateInfo.Coefficient);
            }

            await unitOfWork.SaveAsync();
        }
    }
}
