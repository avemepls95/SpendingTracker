using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
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
            if (request is not IValidated validateObject)
                return await next();

            IEnumerable<IValidator> validators = GetValidators(validateObject.InnerRequest);

            foreach (var validator in validators)
            {
                var validationContext = new ValidationContext<dynamic>(validateObject.InnerRequest);
                var validationResult = await validator.ValidateAsync(
                    validationContext,
                    cancellationToken);

                if (validationResult.IsValid)
                {
                    continue;
                }

                var message = string.Join(
                    Environment.NewLine,
                    validationResult.Errors.Select(e => e.ErrorMessage));

                throw new ValidationException(message);
            }

            return await next();
        }

        private IEnumerable<IValidator> GetValidators<TInnerRequest>(TInnerRequest innerRequest)
        {
            return _serviceProvider.GetServices<IValidator<TInnerRequest>>();
        }
    }
}