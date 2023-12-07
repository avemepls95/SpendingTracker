using Microsoft.Extensions.DependencyInjection;
using SpendingTracker.Application.ExceptionDescriptors;
using SpendingTracker.Application.Factories;
using SpendingTracker.Application.Factories.Abstractions;
using SpendingTracker.Application.Handlers.Spending.Services;
using SpendingTracker.Application.Handlers.Spending.Services.Abstractions;

namespace SpendingTracker.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services
                .AddSingleton<ICategoryFactory, CategoryFactory>()
                .AddScoped<ISpendingsConvertingService, SpendingsConvertingService>()
                .AddExceptionDescriptors();

            return services;
        }
    }
}