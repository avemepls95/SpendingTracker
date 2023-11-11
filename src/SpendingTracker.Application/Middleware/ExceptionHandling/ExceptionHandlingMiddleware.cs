using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpendingTracker.Application.ExceptionDescriptors;
using SpendingTracker.GenericSubDomain.Common;
using SpendingTracker.GenericSubDomain.Validation;

namespace SpendingTracker.Application.Middleware.ExceptionHandling
{
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly IEnumerable<IExceptionDescriptor> _exceptionDescriptors;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IJsonSerializer jsonSerializer,
            IHostEnvironment hostEnvironment,
            IEnumerable<IExceptionDescriptor> exceptionDescriptors)
        {
            _next = next;
            _logger = logger;
            _jsonSerializer = jsonSerializer;
            _hostEnvironment = hostEnvironment;
            _exceptionDescriptors = exceptionDescriptors;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                if (!TryHandle(context, exception))
                {
                    _logger.LogCritical(exception, "Unhandled exception.");

                    var errors = _hostEnvironment.IsDevelopment()
                        ? ConvertToDevelop(exception)
                        : new[]
                        {
                            ErrorProperty.FromCode(ValidationErrorCodeEnum.InternalServerError)
                        };

                    WriteResponse(context, HttpStatusCode.InternalServerError, errors);
                }
            }
        }

        private bool TryHandle(HttpContext context, Exception exception)
        {
            try
            {
                foreach (var exceptionDescriptor in _exceptionDescriptors)
                {
                    if (!exceptionDescriptor.CanHandle(exception))
                        continue;

                    var exceptionResponse = exceptionDescriptor.Handle(exception);

                    WriteResponse(context, exceptionDescriptor.StatusCode, exceptionResponse);
                    // TODO: enrich message
                    _logger.LogInformation("User friendly exception thrown");

                    return true;
                }
            }
            catch (Exception innerException)
            {
                _logger.LogCritical(innerException, "Exception matching was failed.");
            }

            return false;
        }

        private ErrorProperty[] ConvertToDevelop(Exception exception)
        {
            var errors = exception.InnerException == null
                ? new[]
                {
                    ErrorProperty.FromMessage($"{nameof(exception.Message)}: {exception.Message}"),
                    ErrorProperty.FromMessage($"{nameof(exception.StackTrace)}: {exception.StackTrace}")
                }
                : new[]
                {
                    ErrorProperty.FromMessage($"{nameof(exception.Message)}: {exception.Message}"),
                    ErrorProperty.FromMessage($"{nameof(exception.StackTrace)}: {exception.StackTrace}"),
                    ErrorProperty.FromMessage($"{nameof(Exception.InnerException)}_{nameof(Exception.Message)}: {exception.InnerException.Message}"),
                    ErrorProperty.FromMessage($"{nameof(Exception.InnerException)}_{nameof(Exception.StackTrace)}: {exception.InnerException.StackTrace}")
                };

            return errors;
        }

        private void WriteResponse(HttpContext context, HttpStatusCode statusCode, ErrorProperty[] errors)
        {
            var data = _jsonSerializer.Serialize(errors);

            context.Response.Clear();
            context.Response.StatusCode = (int) statusCode;
            context.Response.ContentType = "application/json";
            context.Response.WriteAsync(data);
        }
    }
}