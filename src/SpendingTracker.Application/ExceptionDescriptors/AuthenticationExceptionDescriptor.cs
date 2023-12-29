using System.Net;
using System.Security.Authentication;
using SpendingTracker.Application.Middleware.ExceptionHandling;
using SpendingTracker.GenericSubDomain.Validation;

namespace SpendingTracker.Application.ExceptionDescriptors
{
    internal sealed class AuthenticationExceptionDescriptor : IExceptionDescriptor
    {
        public bool CanHandle(Exception ex)
        {
            return ex is AuthenticationException;
        }

        public HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;

        public ErrorProperty[] Handle(Exception ex)
        {
            var errors = new[]
            {
                ErrorProperty.FromCode(ValidationErrorCodeEnum.Forbidden)
            };

            return errors;
        }
    }
}