using Microsoft.Extensions.DependencyInjection;
using SpendingTracker.TelegramBot.Services.Abstractions;
using SpendingTracker.TelegramBot.Services.ButtonGroupTransformers;

namespace SpendingTracker.TelegramBot
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<ITelegramUserCurrentButtonGroupService, TelegramUserCurrentButtonGroupService>();

            return services;
        }
    }
}