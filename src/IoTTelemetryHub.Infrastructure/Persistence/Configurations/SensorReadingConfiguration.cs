using IoTTelemetryHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IoTTelemetryHub.Infrastructure.Persistence.Configurations;

public class SensorReadingConfiguration : IEntityTypeConfiguration<SensorReading>
{
    // NOTE: once the migration exists, RecordedAtUtc + DeviceId is promoted to
    // a Timescale hypertable via `SELECT create_hypertable(...)` in a raw-SQL
    // migration — see docs/ARCHITECTURE.md.
    public void Configure(EntityTypeBuilder<SensorReading> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Unit).IsRequired().HasMaxLength(16);
        builder.HasIndex(r => new { r.DeviceId, r.RecordedAtUtc });
    }
}
