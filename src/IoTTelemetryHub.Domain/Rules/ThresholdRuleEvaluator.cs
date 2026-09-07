using IoTTelemetryHub.Domain.Entities;
using IoTTelemetryHub.Domain.Enums;

namespace IoTTelemetryHub.Domain.Rules;

public class ThresholdRuleEvaluator : IAlertRuleEvaluator
{
    public AlertRuleType HandlesType => AlertRuleType.Threshold;

    public AlertEvent? Evaluate(AlertRule rule, Device device, IReadOnlyList<SensorReading> recentReadings)
    {
        var latest = recentReadings.MaxBy(r => r.RecordedAtUtc);
        if (latest is null || latest.Value < rule.Threshold)
            return null;

        return new AlertEvent(
            device.Id,
            rule.Id,
            rule.Severity,
            $"{device.Name}: reading {latest.Value}{latest.Unit} exceeded threshold {rule.Threshold}.");
    }
}
