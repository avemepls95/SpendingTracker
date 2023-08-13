using Microsoft.Extensions.DependencyInjection;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.Infrastructure.Repositories;

namespace SpendingTracker.Infrastructure.Services
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<ModificationInfoEntityService>()
                .AddScoped<ITelegramUserCurrentButtonGroupRepository, TelegramUserCurrentButtonGroupRepository>();

            return services;
        }
    }
}