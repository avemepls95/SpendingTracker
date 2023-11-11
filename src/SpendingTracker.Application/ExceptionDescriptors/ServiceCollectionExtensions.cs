using Microsoft.Extensions.DependencyInjection;

namespace SpendingTracker.Application.ExceptionDescriptors
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExceptionDescriptors(this IServiceCollection services)
        {
            services
                .AddSingleton<IExceptionDescriptor, FluentValidationExceptionDescriptor>()
                .AddSingleton<IExceptionDescriptor, KeyNotFoundExceptionDescriptor>()
                .AddSingleton<IExceptionDescriptor, UpdateConcurrencyExceptionDescriptor>()
                .AddSingleton<IExceptionDescriptor, AuthenticationExceptionDescriptor>()
                .AddSingleton<IExceptionDescriptor, SpendingTrackerValidationExceptionDescriptor>()
                ;

            return services;
        }
    }
}