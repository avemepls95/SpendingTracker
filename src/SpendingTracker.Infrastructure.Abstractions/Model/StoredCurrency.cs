using SpendingTracker.Common.Primitives;

namespace SpendingTracker.Infrastructure.Abstractions.Model;

/// <summary>
/// Валюта.
/// </summary>
public class StoredCurrency : EntityObject<StoredCurrency, Guid>
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Кодовое обозначение.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Человекопонятное название.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Является ли валюта дефолтной.
    /// </summary>
    public bool IsDefault { get; set; }

    public override Guid GetKey()
    {
        return Id;
    }
}