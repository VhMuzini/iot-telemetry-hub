using System.Net.Http.Json;
using IoTTelemetryHub.Application.Common.Interfaces;
using IoTTelemetryHub.Domain.Entities;
using IoTTelemetryHub.Domain.Enums;

namespace IoTTelemetryHub.Infrastructure.Notifications;

public class WebhookNotificationChannel : INotificationChannel
{
    private readonly HttpClient _httpClient;

    public WebhookNotificationChannel(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public NotificationChannelType HandlesChannel => NotificationChannelType.Webhook;

    public async Task SendAsync(AlertEvent alertEvent, CancellationToken cancellationToken = default)
    {
        // Base address + auth for the configured webhook endpoint are set up
        // where this HttpClient is registered (see DependencyInjection.cs).
        await _httpClient.PostAsJsonAsync("", new
        {
            alertEvent.Id,
            alertEvent.DeviceId,
            alertEvent.Severity,
            alertEvent.Message
        }, cancellationToken);
    }
}
