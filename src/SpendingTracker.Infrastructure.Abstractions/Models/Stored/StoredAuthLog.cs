using SpendingTracker.Common.Primitives;

namespace SpendingTracker.Infrastructure.Abstractions.Models.Stored;

public class StoredAuthLog
{
    public Guid Id { get; set; }
    public UserKey UserId { get; set; }
    public DateTimeOffset Date { get; set; }
    public AuthSource Source { get; set; }
    public string? AdditionalData { get; set; }
}