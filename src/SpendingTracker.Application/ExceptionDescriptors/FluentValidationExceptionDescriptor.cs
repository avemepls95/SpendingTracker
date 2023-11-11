using System.Net;
using FluentValidation;
using SpendingTracker.Application.Middleware.ExceptionHandling;

namespace SpendingTracker.Application.ExceptionDescriptors
{
    internal sealed class FluentValidationExceptionDescriptor : IExceptionDescriptor
    {
        public bool CanHandle(Exception ex)
        {
            return ex is ValidationException;
        }

        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public ErrorProperty[] Handle(Exception ex)
        {
            var validationException = (ValidationException) ex;

            if (!string.IsNullOrWhiteSpace(validationException.Message))
            {
                return new[]
                {
                    ErrorProperty.FromMessage(validationException.Message)
                };
            }
            
            var errors = validationException.Errors
                .Select(e => ErrorProperty.FromMessage(e.ErrorMessage))
                .ToArray();

            return errors;
        }
    }
}