namespace SpendingTracker.Application.Handlers.Common;

public static class DoubleExtensions
{
    private const double Tolerance = 0.001;
    
    public static bool IsEqual(this double value, double anotherValue)
    {
        return value - anotherValue < Tolerance;
    }
}