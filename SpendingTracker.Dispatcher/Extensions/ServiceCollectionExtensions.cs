using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SpendingTracker.Dispatcher.DataTransfer.Pipelines;

namespace SpendingTracker.Dispatcher.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Добавление медиатра
        /// </summary>
        public static IServiceCollection AddDispatcher(this IServiceCollection services, Assembly[] assembliesForScan)
        {
            services.AddMediatR(a => a.RegisterServicesFromAssemblies(assembliesForScan));
            return services;
        }

        /// <summary>
        /// Добавление слоя промежуточной валидации с помощью FluentValidator
        /// Пример использования:
        /// school-medportal-integration-service / Common / Validators
        /// </summary>
        public static IServiceCollection AddFluentValidation(this IServiceCollection services, Assembly[] assembliesForScan)
        {
            services.TryAddEnumerable(ServiceDescriptor.Scoped(typeof(IPipelineBehavior<,>),
                typeof(ValidationPipelineBehavior<,>)));

            // ValidatorOptions.LanguageManager.Culture = new CultureInfo("ru");
            var assemblyScanner = new AssemblyScanner(assembliesForScan.SelectMany(a => a.GetTypes()));

            assemblyScanner.ForEach(t =>
            {
                services.Add(ServiceDescriptor.Transient(t.InterfaceType, t.ValidatorType));
                services.Add(ServiceDescriptor.Transient(t.ValidatorType, t.ValidatorType));
            });

            return services;
        }
    }
}