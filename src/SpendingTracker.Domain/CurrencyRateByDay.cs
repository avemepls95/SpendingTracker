namespace SpendingTracker.Domain;

public class CurrencyRateByDay
{
    public DateTimeOffset Date { get; set; }
    public Guid Base { get; set; }
    public Guid Target { get; set; }
    public double Coefficient { get; set; }
}