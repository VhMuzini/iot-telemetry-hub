using FluentAssertions;
using IoTTelemetryHub.Domain.Entities;
using IoTTelemetryHub.Domain.Enums;
using IoTTelemetryHub.Domain.Rules;
using Xunit;

namespace IoTTelemetryHub.UnitTests.Domain.Rules;

public class ThresholdRuleEvaluatorTests
{
    [Fact]
    public void Evaluate_WhenLatestReadingExceedsThreshold_FiresAlert()
    {
        var device = new Device("River Sensor 01", "river-level", "Bridge A");
        var rule = new AlertRule("river-level", AlertRuleType.Threshold, threshold: 3.5,
            evaluationWindow: TimeSpan.FromMinutes(10), severity: AlertSeverity.Critical,
            notificationChannel: NotificationChannelType.Webhook);

        var readings = new List<SensorReading>
        {
            new(device.Id, 4.1, "m", DateTimeOffset.UtcNow)
        };

        var evaluator = new ThresholdRuleEvaluator();

        var result = evaluator.Evaluate(rule, device, readings);

        result.Should().NotBeNull();
        result!.Severity.Should().Be(AlertSeverity.Critical);
        result.Message.Should().Contain("4.1");
    }

    [Fact]
    public void Evaluate_WhenLatestReadingBelowThreshold_DoesNotFire()
    {
        var device = new Device("River Sensor 01", "river-level", "Bridge A");
        var rule = new AlertRule("river-level", AlertRuleType.Threshold, threshold: 3.5,
            evaluationWindow: TimeSpan.FromMinutes(10), severity: AlertSeverity.Critical,
            notificationChannel: NotificationChannelType.Webhook);

        var readings = new List<SensorReading>
        {
            new(device.Id, 1.2, "m", DateTimeOffset.UtcNow)
        };

        var evaluator = new ThresholdRuleEvaluator();

        var result = evaluator.Evaluate(rule, device, readings);

        result.Should().BeNull();
    }
}
