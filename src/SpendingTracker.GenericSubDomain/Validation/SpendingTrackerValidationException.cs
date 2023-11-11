namespace SpendingTracker.GenericSubDomain.Validation;

public class SpendingTrackerValidationException : Exception
{
    public ValidationErrorCodeEnum Code { get; }

    public SpendingTrackerValidationException(ValidationErrorCodeEnum code)
    {
        Code = code;
    }
}