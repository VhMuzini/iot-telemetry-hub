using IoTTelemetryHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IoTTelemetryHub.Application.Common.Interfaces;

/// <summary>
/// Application-layer view of the persistence layer. Infrastructure implements
/// this so Application never takes a direct dependency on EF Core internals
/// beyond the DbSet/SaveChanges surface it needs.
/// </summary>
public interface IApplicationDbContext
{
    DbSet<Device> Devices { get; }
    DbSet<SensorReading> SensorReadings { get; }
    DbSet<AlertRule> AlertRules { get; }
    DbSet<AlertEvent> AlertEvents { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
