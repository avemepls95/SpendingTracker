using Microsoft.Extensions.DependencyInjection;

namespace SpendingTracker.Infrastructure.Services
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<ModificationInfoEntityService>();

            return services;
        }
    }
}