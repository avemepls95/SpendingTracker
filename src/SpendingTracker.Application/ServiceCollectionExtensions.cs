using Microsoft.Extensions.DependencyInjection;
using SpendingTracker.Application.Factories;
using SpendingTracker.Application.Factories.Abstractions;

namespace SpendingTracker.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services
                .AddSingleton<ICategoryFactory, CategoryFactory>();

            return services;
        }
    }
}