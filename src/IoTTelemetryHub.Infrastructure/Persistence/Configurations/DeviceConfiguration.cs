using IoTTelemetryHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IoTTelemetryHub.Infrastructure.Persistence.Configurations;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Name).IsRequired().HasMaxLength(200);
        builder.Property(d => d.DeviceType).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Location).HasMaxLength(200);
        builder.HasIndex(d => d.DeviceType);
    }
}
