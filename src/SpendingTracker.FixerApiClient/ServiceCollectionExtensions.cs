using Microsoft.Extensions.DependencyInjection;
using SpendingTracker.ApiClient;

namespace FixerApiClient
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFixerRateApiClient(this IServiceCollection services)
        {
            services
                .AddSingleton<ICurrencyRateApiClient, CurrencyRateApiClient>();

            return services;
        }
    }
}