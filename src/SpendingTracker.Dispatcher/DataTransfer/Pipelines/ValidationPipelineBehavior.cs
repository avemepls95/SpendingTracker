using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SpendingTracker.Common;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Dispatcher.DataTransfer.Pipelines
{
    /// <summary>
    /// Валидатор для медиатра
    /// в случае ошибки валидации получим исключение <see cref="SchoolValidationException"/>
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidationPipelineBehavior(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!(request is IValidated validateObject))
                return await next();

            IEnumerable<IValidator> validators = GetValidators(validateObject.InnerRequest);

            foreach (var validator in validators)
            {
                ValidationResult validationResult = await validator.ValidateAsync(
                    validateObject.InnerRequest,
                    cancellationToken);

                if (validationResult.IsValid)
                {
                    continue;
                }

                var message = string.Join(
                    Environment.NewLine,
                    validationResult.Errors.Select(e => e.ErrorMessage));

                throw new SpendingTrackerException(message);
            }

            return await next();
        }

        private IEnumerable<IValidator> GetValidators<TInnerRequest>(TInnerRequest innerRequest)
        {
            return _serviceProvider.GetServices<IValidator<TInnerRequest>>();
        }
    }
}