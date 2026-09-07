using IoTTelemetryHub.Domain.Entities;
using IoTTelemetryHub.Domain.Enums;

namespace IoTTelemetryHub.Domain.Rules;

public class DeviceOfflineRuleEvaluator : IAlertRuleEvaluator
{
    public AlertRuleType HandlesType => AlertRuleType.DeviceOffline;

    public AlertEvent? Evaluate(AlertRule rule, Device device, IReadOnlyList<SensorReading> recentReadings)
    {
        if (device.LastSeenAtUtc is null)
            return null;

        var silentFor = DateTimeOffset.UtcNow - device.LastSeenAtUtc.Value;
        if (silentFor < rule.EvaluationWindow)
            return null;

        return new AlertEvent(
            device.Id,
            rule.Id,
            rule.Severity,
            $"{device.Name}: no reading received for {silentFor.TotalMinutes:F0} minutes.");
    }
}
