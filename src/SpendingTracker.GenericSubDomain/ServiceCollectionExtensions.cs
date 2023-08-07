using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpendingTracker.GenericSubDomain.User;
using SpendingTracker.GenericSubDomain.User.Internal;

namespace SpendingTracker.GenericSubDomain
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGenericSubDomain(this IServiceCollection services, IConfiguration configuration)
        {
            var optionsSection = configuration.GetSection(nameof(SystemUserContextOptions));
            services.Configure<SystemUserContextOptions>(optionsSection);

            services
                .AddScoped<IUserContextFactory, SystemUserContextFactory>()
                .AddScoped<ICurrentUserProvider, CurrentUserProvider>();
            services.AddScoped<UserContextFactoryProvider>();

            return services;
        }
    }
}