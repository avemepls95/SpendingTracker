using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.Infrastructure.Repositories;
using SpendingTracker.Infrastructure.Services;

namespace SpendingTracker.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionsStrings = configuration.GetSection(nameof(ConnectionStrings)).Get<ConnectionStrings>()!;

            services
                .AddSingleton(connectionsStrings)
                .AddDbContextPool<MainDbContext>(
                    o => o.UseNpgsql(connectionsStrings.DbConnectionString));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddServices();

            services.AddScoped<ISpendingRepository, SpendingRepository>();
            services.AddScoped<IUserCurrencyRepository, UserCurrencyRepository>();

            return services;
        }
    }
}