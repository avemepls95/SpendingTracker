using SpendingTracker.Common.Primitives;

namespace SpendingTracker.Domain;

/// <summary>
/// Валюта.
/// </summary>
public class Currency : EntityObject<Currency, Guid>
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Кодовое обозначение.
    /// </summary>
    public string Code { get; init; }

    /// <summary>
    /// Иконка страны
    /// </summary>
    public string CountryIcon { get; set; }

    /// <summary>
    /// Человекопонятное название.
    /// </summary>
    public string Title { get; init; }

    /// <summary>
    /// Признак удаленности.
    /// </summary>
    public bool IsDeleted { get; set; }

    public override Guid GetKey()
    {
        return Id;
    }
}