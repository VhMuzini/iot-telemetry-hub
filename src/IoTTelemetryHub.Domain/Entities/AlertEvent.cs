using IoTTelemetryHub.Domain.Common;
using IoTTelemetryHub.Domain.Enums;

namespace IoTTelemetryHub.Domain.Entities;

public class AlertEvent : BaseEntity
{
    public Guid DeviceId { get; private set; }
    public Guid AlertRuleId { get; private set; }
    public AlertSeverity Severity { get; private set; }
    public string Message { get; private set; }
    public bool Dispatched { get; private set; }

    private AlertEvent()
    {
        Message = string.Empty;
    }

    public AlertEvent(Guid deviceId, Guid alertRuleId, AlertSeverity severity, string message)
    {
        DeviceId = deviceId;
        AlertRuleId = alertRuleId;
        Severity = severity;
        Message = message;
    }

    public void MarkDispatched() => Dispatched = true;
}
