namespace SpendingTracker.Common;

public class SpendingTrackerException : Exception
{
    public SpendingTrackerException(string message)
        : base(message)
    {
    }
}