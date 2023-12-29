using Microsoft.Extensions.DependencyInjection;
using SpendingTracker.GenericSubDomain.Common;
using SpendingTracker.GenericSubDomain.StartupTasks.Abstractions;
using SpendingTracker.GenericSubDomain.User;
using SpendingTracker.GenericSubDomain.User.Abstractions;
using SpendingTracker.GenericSubDomain.User.Internal;

namespace SpendingTracker.GenericSubDomain
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGenericSubDomain(
            this IServiceCollection services,
            TelegramOptions telegramOptions)
        {
            services.AddSingleton<TelegramOptions>(_ => telegramOptions);

            services
                .AddScoped<IUserContextFactory, SystemUserContextFactory>()
                .AddScoped<IUserContextFactory, UserByTelegramContextFactory>()
                .AddScoped<ICurrentUserIdProvider, CurrentUserIdProvider>();

            services.AddScoped<ITelegramUserIdStore, TelegramUserIdStore>();
            services.AddScoped<IUserContextFactoryProvider, UserContextFactoryProvider>();

            services.AddSingleton<IJsonSerializer, NewtonsoftJsonSerializer>();

            return services;
        }
        
        public static IServiceCollection AddStartupTask<T>(this IServiceCollection services)
            where T : class, IStartupTask
        {
            return services.AddTransient<IStartupTask, T>();
        }
    }
}