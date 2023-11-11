using System.Net;
using Microsoft.EntityFrameworkCore;
using SpendingTracker.Application.Middleware.ExceptionHandling;
using SpendingTracker.GenericSubDomain.Validation;

namespace SpendingTracker.Application.ExceptionDescriptors
{
    public class UpdateConcurrencyExceptionDescriptor : IExceptionDescriptor
    {
        public bool CanHandle(Exception ex)
        {
            return ex is DbUpdateConcurrencyException;
        }

        public HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public ErrorProperty[] Handle(Exception ex)
        {
            var errors = new[]
            {
                ErrorProperty.FromCode(ValidationErrorCodeEnum.ObjectWasChanged)
            };

            return errors;
        }
    }
}