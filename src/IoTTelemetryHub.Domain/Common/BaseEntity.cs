namespace IoTTelemetryHub.Domain.Common;

/// <summary>
/// Base type for all domain entities. Identity is a GUID assigned at creation
/// time so entities can be safely created offline (e.g. by a device before it
/// ever talks to the API) without waiting on a database-generated key.
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();

    public DateTimeOffset CreatedAtUtc { get; protected set; } = DateTimeOffset.UtcNow;

    private readonly List<object> _domainEvents = new();

    public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(object domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}
