using IoTTelemetryHub.Domain.Common;
using IoTTelemetryHub.Domain.Enums;

namespace IoTTelemetryHub.Domain.Entities;

/// <summary>
/// A configurable condition attached to a device type. Kept intentionally
/// generic (a numeric threshold plus a time window) so the same entity can
/// back threshold, rate-of-change and offline-detection rules — the
/// interpretation of the fields is up to the matching IAlertRuleEvaluator.
/// </summary>
public class AlertRule : BaseEntity
{
    public string DeviceType { get; private set; }
    public AlertRuleType Type { get; private set; }
    public double Threshold { get; private set; }
    public TimeSpan EvaluationWindow { get; private set; }
    public AlertSeverity Severity { get; private set; }
    public NotificationChannelType NotificationChannel { get; private set; }
    public bool IsEnabled { get; private set; } = true;

    private AlertRule()
    {
        DeviceType = string.Empty;
    }

    public AlertRule(
        string deviceType,
        AlertRuleType type,
        double threshold,
        TimeSpan evaluationWindow,
        AlertSeverity severity,
        NotificationChannelType notificationChannel)
    {
        DeviceType = deviceType;
        Type = type;
        Threshold = threshold;
        EvaluationWindow = evaluationWindow;
        Severity = severity;
        NotificationChannel = notificationChannel;
    }

    public void Disable() => IsEnabled = false;

    public void Enable() => IsEnabled = true;
}
