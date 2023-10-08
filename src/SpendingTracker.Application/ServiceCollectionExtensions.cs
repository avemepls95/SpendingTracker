using Microsoft.Extensions.DependencyInjection;
using SpendingTracker.Application.ExceptionDescriptors;
using SpendingTracker.Application.Factories;
using SpendingTracker.Application.Factories.Abstractions;
using SpendingTracker.CurrencyRate;

namespace SpendingTracker.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services
                .AddSingleton<ICategoryFactory, CategoryFactory>()
                .AddExceptionDescriptors()
                .AddCurrencyRates();

            return services;
        }
    }
}