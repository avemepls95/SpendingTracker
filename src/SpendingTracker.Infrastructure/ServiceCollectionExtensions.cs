using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
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
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionsStrings = configuration.GetSection(nameof(ConnectionStrings)).Get<ConnectionStrings>()!;
            if (string.IsNullOrWhiteSpace(connectionsStrings.SpendingTrackerDb))
            {
                connectionsStrings.SpendingTrackerDb = Environment.GetEnvironmentVariable("CONNECTION-STRINGS_SPENDING-TRACKER-DB");
            }

            var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
            
            services
                .AddSingleton(connectionsStrings)
                .AddDbContextPool<MainDbContext>(
                    o => o.UseNpgsql(connectionsStrings.SpendingTrackerDb)
                        .UseLoggerFactory(loggerFactory)
                        .EnableSensitiveDataLogging()
                        .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored)));

            services.AddDbContextFactory<MainDbContext>(options => options
                .UseNpgsql(connectionsStrings.SpendingTrackerDb)
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
                .AddScoped<ICurrencyRateByDayRepository, CurrencyRateByDayRepository>();

            services
                .AddSingleton<ICurrencyFactory, CurrencyFactory>()
                .AddSingleton<ISpendingFactory, SpendingFactory>()
                .AddSingleton<ICategoryFactory, CategoryFactory>()
                .AddSingleton<IUserFactory, UserFactory>();

            return services;
        }
    }
}