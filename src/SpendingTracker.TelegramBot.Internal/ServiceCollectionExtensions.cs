using Microsoft.Extensions.DependencyInjection;
using SpendingTracker.TelegramBot.Internal.Abstractions;
using SpendingTracker.TelegramBot.Internal.Buttons;

namespace SpendingTracker.TelegramBot.Internal
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTelegramBotWrappingServices(this IServiceCollection services)
        {
            services
                .AddSingleton<IButtonsGroupManager, ButtonsGroupManager>();

            return services;
        }
    }
}