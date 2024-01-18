namespace SpendingTracker.GenericSubDomain.Validation;

public class SpendingTrackerValidationException : Exception
{
    public ValidationErrorCodeEnum Code { get; }
    public object? Data { get; set; }

    public SpendingTrackerValidationException(ValidationErrorCodeEnum code, object? data = null)
    {
        Code = code;
        Data = data;
    }
}