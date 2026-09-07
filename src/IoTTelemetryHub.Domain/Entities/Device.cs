using IoTTelemetryHub.Domain.Common;
using IoTTelemetryHub.Domain.Enums;

namespace IoTTelemetryHub.Domain.Entities;

public class Device : BaseEntity
{
    public string Name { get; private set; }
    public string DeviceType { get; private set; }
    public string Location { get; private set; }
    public DeviceStatus Status { get; private set; } = DeviceStatus.Offline;
    public DateTimeOffset? LastSeenAtUtc { get; private set; }

    private Device()
    {
        Name = string.Empty;
        DeviceType = string.Empty;
        Location = string.Empty;
    }

    public Device(string name, string deviceType, string location)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Device name is required.", nameof(name));

        Name = name;
        DeviceType = deviceType;
        Location = location;
    }

    public void RegisterReadingReceived(DateTimeOffset atUtc)
    {
        LastSeenAtUtc = atUtc;
        Status = DeviceStatus.Online;
    }

    public void MarkOffline() => Status = DeviceStatus.Offline;

    public void MarkDegraded() => Status = DeviceStatus.Degraded;
}
