using FixerApiClient;
using Microsoft.Extensions.DependencyInjection;
using SpendingTracker.CurrencyRate.Abstractions;
using SpendingTracker.CurrencyRate.BackgroundServices;

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

            return services;
        }
    }
}