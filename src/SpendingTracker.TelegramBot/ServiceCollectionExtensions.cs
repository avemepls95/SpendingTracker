using Microsoft.Extensions.DependencyInjection;
using SpendingTracker.TelegramBot.Services;
using SpendingTracker.TelegramBot.Services.Abstractions;

namespace SpendingTracker.TelegramBot
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<GatewayService>()
                .AddScoped<ITelegramUserCurrentButtonGroupService, TelegramUserCurrentButtonGroupService>();

            return services;
        }
    }
}