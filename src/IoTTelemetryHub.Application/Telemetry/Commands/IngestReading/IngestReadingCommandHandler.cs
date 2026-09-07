using IoTTelemetryHub.Application.Common.Interfaces;
using IoTTelemetryHub.Domain.Entities;
using IoTTelemetryHub.Domain.Rules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IoTTelemetryHub.Application.Telemetry.Commands.IngestReading;

/// <summary>
/// The core ingestion flow: persist the reading, mark the device as seen,
/// evaluate only the alert rules relevant to this device's type against its
/// recent history, and dispatch/broadcast anything that fires.
/// </summary>
public class IngestReadingCommandHandler : IRequestHandler<IngestReadingCommand, Guid>
{
    private readonly IApplicationDbContext _db;
    private readonly IEnumerable<IAlertRuleEvaluator> _evaluators;
    private readonly INotificationDispatcher _notificationDispatcher;
    private readonly IRealtimeNotifier _realtimeNotifier;

    public IngestReadingCommandHandler(
        IApplicationDbContext db,
        IEnumerable<IAlertRuleEvaluator> evaluators,
        INotificationDispatcher notificationDispatcher,
        IRealtimeNotifier realtimeNotifier)
    {
        _db = db;
        _evaluators = evaluators;
        _notificationDispatcher = notificationDispatcher;
        _realtimeNotifier = realtimeNotifier;
    }

    public async Task<Guid> Handle(IngestReadingCommand request, CancellationToken cancellationToken)
    {
        var device = await _db.Devices.FirstOrDefaultAsync(d => d.Id == request.DeviceId, cancellationToken)
            ?? throw new KeyNotFoundException($"Device '{request.DeviceId}' is not registered.");

        var reading = new SensorReading(device.Id, request.Value, request.Unit, request.RecordedAtUtc);
        _db.SensorReadings.Add(reading);
        device.RegisterReadingReceived(request.RecordedAtUtc);

        await _db.SaveChangesAsync(cancellationToken);
        await _realtimeNotifier.NotifyReadingReceivedAsync(reading, cancellationToken);

        var rules = await _db.AlertRules
            .Where(r => r.DeviceType == device.DeviceType && r.IsEnabled)
            .ToListAsync(cancellationToken);

        foreach (var rule in rules)
        {
            var evaluator = _evaluators.FirstOrDefault(e => e.HandlesType == rule.Type);
            if (evaluator is null)
                continue;

            var window = DateTimeOffset.UtcNow - rule.EvaluationWindow;
            var recentReadings = await _db.SensorReadings
                .Where(r => r.DeviceId == device.Id && r.RecordedAtUtc >= window)
                .ToListAsync(cancellationToken);

            var alertEvent = evaluator.Evaluate(rule, device, recentReadings);
            if (alertEvent is null)
                continue;

            _db.AlertEvents.Add(alertEvent);
            await _db.SaveChangesAsync(cancellationToken);

            await _notificationDispatcher.DispatchAsync(alertEvent, cancellationToken);
            await _realtimeNotifier.NotifyAlertFiredAsync(alertEvent, cancellationToken);
        }

        return reading.Id;
    }
}
