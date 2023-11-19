using FixerApiClient;
using Microsoft.Extensions.DependencyInjection;
using SpendingTracker.CurrencyRate.Abstractions;
using SpendingTracker.CurrencyRate.BackgroundServices;
using SpendingTracker.CurrencyRate.StartupTasks;
using SpendingTracker.GenericSubDomain;

namespace SpendingTracker.CurrencyRate
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCurrencyRates(this IServiceCollection services)
        {
            services
                .AddScoped<ICurrencyConverter, CurrencyConverter>()
                .AddSingleton<IRatesProvider, RatesProvider>()
                .AddHostedService<CurrencyBackgroundService>()
                .AddFixerRateApiClient();

            services
                .AddStartupTask<ActualizeCurrencyRatesByDaysStartupTask>();

            return services;
        }
    }
}