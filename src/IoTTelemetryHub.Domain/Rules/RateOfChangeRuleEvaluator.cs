using IoTTelemetryHub.Domain.Entities;
using IoTTelemetryHub.Domain.Enums;

namespace IoTTelemetryHub.Domain.Rules;

public class RateOfChangeRuleEvaluator : IAlertRuleEvaluator
{
    public AlertRuleType HandlesType => AlertRuleType.RateOfChange;

    public AlertEvent? Evaluate(AlertRule rule, Device device, IReadOnlyList<SensorReading> recentReadings)
    {
        if (recentReadings.Count < 2)
            return null;

        var ordered = recentReadings.OrderBy(r => r.RecordedAtUtc).ToList();
        var first = ordered.First();
        var last = ordered.Last();

        var elapsedMinutes = (last.RecordedAtUtc - first.RecordedAtUtc).TotalMinutes;
        if (elapsedMinutes <= 0)
            return null;

        var ratePerMinute = Math.Abs(last.Value - first.Value) / elapsedMinutes;
        if (ratePerMinute < rule.Threshold)
            return null;

        return new AlertEvent(
            device.Id,
            rule.Id,
            rule.Severity,
            $"{device.Name}: rate of change {ratePerMinute:F2}/min exceeded threshold {rule.Threshold}/min.");
    }
}
