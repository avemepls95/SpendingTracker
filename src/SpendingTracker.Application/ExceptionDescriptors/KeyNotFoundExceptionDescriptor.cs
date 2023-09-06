using System.Net;
using SpendingTracker.Application.Middleware.ExceptionHandling;

namespace SpendingTracker.Application.ExceptionDescriptors
{
    internal sealed class KeyNotFoundExceptionDescriptor : IExceptionDescriptor
    {
        public bool CanHandle(Exception ex)
        {
            return ex is KeyNotFoundException;
        }

        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public ErrorResult Handle(Exception ex)
        {
            var errors = new[]
            {
                new ErrorProperty(nameof(HttpStatusCode.NotFound), ex.Message)
            };

            return new ErrorResult(errors);
        }
    }
}