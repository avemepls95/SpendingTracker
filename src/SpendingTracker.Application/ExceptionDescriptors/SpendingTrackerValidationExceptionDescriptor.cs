using System.Net;
using SpendingTracker.Application.Middleware.ExceptionHandling;
using SpendingTracker.GenericSubDomain.Validation;

namespace SpendingTracker.Application.ExceptionDescriptors
{
    internal sealed class SpendingTrackerValidationExceptionDescriptor : IExceptionDescriptor
    {
        public bool CanHandle(Exception ex)
        {
            return ex is SpendingTrackerValidationException;
        }

        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public ErrorProperty[] Handle(Exception ex)
        {
            var validationException = (SpendingTrackerValidationException) ex;

            return validationException.Data switch
            {
                null => new[] { ErrorProperty.FromCode(validationException.Code) },
                not null => new[] { ErrorProperty.FromCode(validationException.Code, validationException.Data) }
            };
        }
    }
}