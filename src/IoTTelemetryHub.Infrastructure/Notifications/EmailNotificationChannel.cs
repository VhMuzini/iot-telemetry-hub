using IoTTelemetryHub.Application.Common.Interfaces;
using IoTTelemetryHub.Domain.Entities;
using IoTTelemetryHub.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace IoTTelemetryHub.Infrastructure.Notifications;

/// <summary>
/// Placeholder email sender — swap the body of SendAsync for a real provider
/// (SendGrid, SES, SMTP) when wiring this up for real. Kept dependency-free
/// here so the project builds without extra credentials.
/// </summary>
public class EmailNotificationChannel : INotificationChannel
{
    private readonly ILogger<EmailNotificationChannel> _logger;

    public EmailNotificationChannel(ILogger<EmailNotificationChannel> logger)
    {
        _logger = logger;
    }

    public NotificationChannelType HandlesChannel => NotificationChannelType.Email;

    public Task SendAsync(AlertEvent alertEvent, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("[email] Alert {AlertId}: {Message}", alertEvent.Id, alertEvent.Message);
        return Task.CompletedTask;
    }
}
