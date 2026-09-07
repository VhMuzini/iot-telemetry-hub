using IoTTelemetryHub.Domain.Common;

namespace IoTTelemetryHub.Domain.Entities;

public class SensorReading : BaseEntity
{
    public Guid DeviceId { get; private set; }
    public double Value { get; private set; }
    public string Unit { get; private set; }
    public DateTimeOffset RecordedAtUtc { get; private set; }

    private SensorReading()
    {
        Unit = string.Empty;
    }

    public SensorReading(Guid deviceId, double value, string unit, DateTimeOffset recordedAtUtc)
    {
        DeviceId = deviceId;
        Value = value;
        Unit = unit;
        RecordedAtUtc = recordedAtUtc;
    }
}
