using IoTTelemetryHub.Domain.Entities;
using IoTTelemetryHub.Domain.Enums;

namespace IoTTelemetryHub.Domain.Rules;

/// <summary>
/// One implementation per AlertRuleType. New rule types are added by adding a
/// new evaluator, not by touching the ingestion pipeline (open/closed).
/// </summary>
public interface IAlertRuleEvaluator
{
    AlertRuleType HandlesType { get; }

    /// <summary>
    /// Evaluates the rule against the device's recent reading history
    /// (already scoped to the rule's EvaluationWindow by the caller).
    /// Returns an AlertEvent if the rule fired, otherwise null.
    /// </summary>
    AlertEvent? Evaluate(AlertRule rule, Device device, IReadOnlyList<SensorReading> recentReadings);
}
