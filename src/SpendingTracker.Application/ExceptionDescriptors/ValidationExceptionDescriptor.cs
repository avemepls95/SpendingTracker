using System.Net;
using FluentValidation;
using SpendingTracker.Application.Middleware.ExceptionHandling;

namespace SpendingTracker.Application.ExceptionDescriptors
{
    internal sealed class ValidationExceptionDescriptor : IExceptionDescriptor
    {
        public bool CanHandle(Exception ex)
        {
            return ex is ValidationException;
        }

        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public ErrorResult Handle(Exception ex)
        {
            var validationException = (ValidationException) ex;

            if (!string.IsNullOrWhiteSpace(validationException.Message))
            {
                return new ErrorResult(new[]
                {
                    new ErrorProperty(nameof(HttpStatusCode.BadRequest), validationException.Message)
                });
            }
            
            var errors = validationException.Errors
                .Select(e => new ErrorProperty(e.PropertyName, e.ErrorMessage))
                .ToArray();

            return new ErrorResult(errors);
        }
    }
}