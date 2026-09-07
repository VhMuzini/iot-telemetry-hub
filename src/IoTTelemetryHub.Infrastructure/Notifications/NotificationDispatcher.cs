using IoTTelemetryHub.Application.Common.Interfaces;
using IoTTelemetryHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IoTTelemetryHub.Infrastructure.Notifications;

/// <summary>
/// Strategy dispatcher: looks up which channel the firing rule was configured
/// for and delegates to the matching INotificationChannel implementation.
/// </summary>
public class NotificationDispatcher : INotificationDispatcher
{
    private readonly IApplicationDbContext _db;
    private readonly IEnumerable<INotificationChannel> _channels;

    public NotificationDispatcher(IApplicationDbContext db, IEnumerable<INotificationChannel> channels)
    {
        _db = db;
        _channels = channels;
    }

    public async Task DispatchAsync(AlertEvent alertEvent, CancellationToken cancellationToken = default)
    {
        var rule = await _db.AlertRules.FirstOrDefaultAsync(r => r.Id == alertEvent.AlertRuleId, cancellationToken);
        if (rule is null)
            return;

        var channel = _channels.FirstOrDefault(c => c.HandlesChannel == rule.NotificationChannel);
        if (channel is null)
            return;

        await channel.SendAsync(alertEvent, cancellationToken);
        alertEvent.MarkDispatched();
        await _db.SaveChangesAsync(cancellationToken);
    }
}
