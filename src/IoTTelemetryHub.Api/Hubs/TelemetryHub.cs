using Microsoft.AspNetCore.SignalR;

namespace IoTTelemetryHub.Api.Hubs;

/// <summary>
/// Dashboards connect here to receive live readings and alert broadcasts.
/// No server-invocable methods are exposed yet — this is a push-only hub for
/// now, hence the empty body.
/// </summary>
public class TelemetryHub : Hub
{
}
