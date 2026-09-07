using IoTTelemetryHub.Api.Hubs;
using IoTTelemetryHub.Application.Common.Interfaces;
using IoTTelemetryHub.Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace IoTTelemetryHub.Api.Realtime;

public class SignalRRealtimeNotifier : IRealtimeNotifier
{
    private readonly IHubContext<TelemetryHub> _hubContext;

    public SignalRRealtimeNotifier(IHubContext<TelemetryHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task NotifyReadingReceivedAsync(SensorReading reading, CancellationToken cancellationToken = default) =>
        _hubContext.Clients.All.SendAsync("ReadingReceived", new
        {
            reading.DeviceId,
            reading.Value,
            reading.Unit,
            reading.RecordedAtUtc
        }, cancellationToken);

    public Task NotifyAlertFiredAsync(AlertEvent alertEvent, CancellationToken cancellationToken = default) =>
        _hubContext.Clients.All.SendAsync("AlertFired", new
        {
            alertEvent.DeviceId,
            alertEvent.Severity,
            alertEvent.Message
        }, cancellationToken);
}
