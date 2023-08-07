namespace SpendingTracker.Domain;

public class Spending
{
    public double Amount { get; set; }

    public Currency Currency { get; set; }

    public DateTimeOffset Date { get; set; }

    public string Description { get; set; }
}