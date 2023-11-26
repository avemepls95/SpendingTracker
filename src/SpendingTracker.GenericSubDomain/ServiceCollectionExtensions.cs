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
            SystemUserContextOptions systemUserContextOptions,
            TelegramUserContextOptions telegramUserContextOptions)
        {
            services.AddSingleton<SystemUserContextOptions>(_ => systemUserContextOptions);
            services.AddSingleton<TelegramUserContextOptions>(_ => telegramUserContextOptions);

            return AddGenericSubDomain(services);
        }
        
        public static IServiceCollection AddGenericSubDomain(this IServiceCollection services)
        {
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