using SpendingTracker.GenericSubDomain.Validation;

namespace SpendingTracker.Application.Middleware.ExceptionHandling
{
    public sealed class ErrorProperty
    {
        public string Code { get; }
        public string Message { get; }
        public bool MessageIsCustom { get; }

        private ErrorProperty(ValidationErrorCodeEnum code)
        {
            Code = code.ToString();
            MessageIsCustom = false;
        }
        
        private ErrorProperty(string message)
        {
            Message = message;
            MessageIsCustom = true;
        }

        public static ErrorProperty FromCode(ValidationErrorCodeEnum code)
        {
            return new ErrorProperty(code);
        }
        
        public static ErrorProperty FromMessage(string message)
        {
            return new ErrorProperty(message);
        }
    }
}