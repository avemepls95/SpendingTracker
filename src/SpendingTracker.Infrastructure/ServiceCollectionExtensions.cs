using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.Infrastructure.Factories;
using SpendingTracker.Infrastructure.Factories.Abstractions;
using SpendingTracker.Infrastructure.Repositories;
using SpendingTracker.Infrastructure.Services;

namespace SpendingTracker.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            ConnectionStrings connectionStrings)
        {
            var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
            
            services
                .AddSingleton(connectionStrings)
                .AddDbContextPool<MainDbContext>(
                    o => o.UseNpgsql(connectionStrings.SpendingTrackerDb)
                        .UseLoggerFactory(loggerFactory)
                        .EnableSensitiveDataLogging()
                        .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored)));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddServices();

            services
                .AddScoped<ISpendingRepository, SpendingRepository>()
                .AddScoped<IUserCurrencyRepository, UserCurrencyRepository>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<ICurrencyRepository, CurrencyRepository>()
                .AddScoped<ICategoryRepository, CategoryRepository>()
                .AddScoped<ISpendingCategoryLinksRepository, SpendingCategoryLinksRepository>()
                .AddScoped<ICurrencyRateByDayRepository, CurrencyRateByDayRepository>()
                .AddScoped<IUserSettingsRepository, UserSettingsRepository>()
                .AddScoped<IUserSettingValuesRepository, UserSettingValuesRepository>()
                .AddScoped<IAccountRepository, AccountRepository>()
                .AddScoped<IAuthLogRepository, AuthLogRepository>()
                .AddScoped<IIncomeRepository, IncomeRepository>();

            services
                .AddSingleton<ICurrencyFactory, CurrencyFactory>()
                .AddSingleton<ISpendingFactory, SpendingFactory>()
                .AddSingleton<ICategoryFactory, CategoryFactory>()
                .AddSingleton<IUserFactory, UserFactory>()
                .AddSingleton<IUserSettingValueFactory, UserSettingValueFactory>()
                .AddSingleton<IUserSettingFactory, UserSettingFactory>();

            services.AddScoped<IDataInitializer, DataInitializer>();

            return services;
        }
    }
}