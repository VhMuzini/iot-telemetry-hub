using IoTTelemetryHub.Domain.Entities;

namespace IoTTelemetryHub.Application.Common.Interfaces;

public interface INotificationDispatcher
{
    Task DispatchAsync(AlertEvent alertEvent, CancellationToken cancellationToken = default);
}

/// <summary>
/// One implementation per NotificationChannelType. The dispatcher (in
/// Infrastructure) picks the right channel for the rule that fired.
/// </summary>
public interface INotificationChannel
{
    Domain.Enums.NotificationChannelType HandlesChannel { get; }

    Task SendAsync(AlertEvent alertEvent, CancellationToken cancellationToken = default);
}
