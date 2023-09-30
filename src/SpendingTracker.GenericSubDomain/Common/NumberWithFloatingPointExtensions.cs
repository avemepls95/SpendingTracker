namespace SpendingTracker.GenericSubDomain.Common;

public static class NumberWithFloatingPointExtensions
{
    private const double Precision = 0.000001;
    
    public static bool IsEqual(this float number1, float number2)
    {
        return Math.Abs(number1 - number2) < Precision;
    }
}