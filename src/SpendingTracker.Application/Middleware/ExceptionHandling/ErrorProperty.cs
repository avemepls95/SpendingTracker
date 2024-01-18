using SpendingTracker.GenericSubDomain.Validation;

namespace SpendingTracker.Application.Middleware.ExceptionHandling
{
    public sealed class ErrorProperty
    {
        public string Code { get; }
        public string Message { get; }
        public bool MessageIsCustom { get; }
        public object? Data { get; }

        private ErrorProperty(ValidationErrorCodeEnum code, object? data = null)
        {
            Code = code.ToString();
            MessageIsCustom = false;
            Data = data;
        }
        
        private ErrorProperty(string message)
        {
            Message = message;
            MessageIsCustom = true;
        }

        public static ErrorProperty FromCode(ValidationErrorCodeEnum code, object? data = null)
        {
            return new ErrorProperty(code, data);
        }
        
        public static ErrorProperty FromMessage(string message)
        {
            return new ErrorProperty(message);
        }
    }
}