using System.Net;
using System.Security.Authentication;
using SpendingTracker.Application.Middleware.ExceptionHandling;

namespace SpendingTracker.Application.ExceptionDescriptors
{
    internal sealed class AuthenticationExceptionDescriptor : IExceptionDescriptor
    {
        public bool CanHandle(Exception ex)
        {
            return ex is AuthenticationException;
        }

        public HttpStatusCode StatusCode => HttpStatusCode.Forbidden;

        public ErrorResult Handle(Exception ex)
        {
            var errors = new[]
            {
                new ErrorProperty(nameof(HttpStatusCode.Forbidden), ex.Message)
            };

            return new ErrorResult(errors);
        }
    }
}