using System.Net;
using Microsoft.EntityFrameworkCore;
using SpendingTracker.Application.Middleware.ExceptionHandling;

namespace SpendingTracker.Application.ExceptionDescriptors
{
    public class UpdateConcurrencyExceptionDescriptor : IExceptionDescriptor
    {
        public bool CanHandle(Exception ex)
        {
            return ex is DbUpdateConcurrencyException;
        }

        public HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public ErrorResult Handle(Exception ex)
        {
            var errors = new[]
            {
                new ErrorProperty(
                    nameof(HttpStatusCode.Conflict),
                "С последней загрузки объект был изменен. Обновите страницу.")
            };

            return new ErrorResult(errors);
        }
    }
}