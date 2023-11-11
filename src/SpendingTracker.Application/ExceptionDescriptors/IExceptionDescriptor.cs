using System.Net;
using SpendingTracker.Application.Middleware.ExceptionHandling;

namespace SpendingTracker.Application.ExceptionDescriptors
{
    public interface IExceptionDescriptor
    {
        bool CanHandle(Exception ex);

        HttpStatusCode StatusCode { get; }

        ErrorProperty[] Handle(Exception ex);
    }
}
