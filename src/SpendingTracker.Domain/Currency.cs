namespace SpendingTracker.Domain;

/// <summary>
/// Валюта.
/// </summary>
public class Currency
{
    /// <summary>
    /// Кодовое обозначение.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Человекопонятное название.
    /// </summary>
    public string Title { get; set; }
}