namespace SpendingTracker.ApiClient;

public class ExternalCallResult<T> where T: class
{
    public readonly T Result;

    public readonly bool IsSuccess;
    
    public string ErrorMessage { get; }

    private ExternalCallResult(T result)
    {
        Result = result;
        IsSuccess = true;
    }
    
    private ExternalCallResult(string errorMessage)
    {
        ErrorMessage = errorMessage;
        IsSuccess = true;
    }
    
    public static ExternalCallResult<T> CreateSuccess(T result)
    {
        return new ExternalCallResult<T>(result);
    }
    
    public static ExternalCallResult<T> CreateError(string errorMessage)
    {
        return new ExternalCallResult<T>(errorMessage);
    }
}