using IoTTelemetryHub.Domain.Entities;

namespace IoTTelemetryHub.Application.Common.Interfaces;

/// <summary>
/// Pushes live updates to connected dashboards. Implemented in the Api
/// project (over SignalR) so Application stays free of ASP.NET Core
/// dependencies — only the abstraction lives here.
/// </summary>
public interface IRealtimeNotifier
{
    Task NotifyReadingReceivedAsync(SensorReading reading, CancellationToken cancellationToken = default);

    Task NotifyAlertFiredAsync(AlertEvent alertEvent, CancellationToken cancellationToken = default);
}
