using Microsoft.EntityFrameworkCore;
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

            var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
            
            services
                .AddSingleton(connectionsStrings)
                .AddDbContextPool<MainDbContext>(
                    o => o.UseNpgsql(connectionsStrings.DbConnectionString)
                        .UseLoggerFactory(loggerFactory)
                        .EnableSensitiveDataLogging());

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddServices();

            services
                .AddScoped<ISpendingRepository, SpendingRepository>()
                .AddScoped<IUserCurrencyRepository, UserCurrencyRepository>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<ICurrencyRepository, CurrencyRepository>();

            services
                .AddSingleton<ICurrencyFactory, CurrencyFactory>()
                .AddSingleton<ISpendingFactory, SpendingFactory>()
                .AddSingleton<ICategoryFactory, CategoryFactory>()
                .AddSingleton<IUserFactory, UserFactory>();

            return services;
        }
    }
}