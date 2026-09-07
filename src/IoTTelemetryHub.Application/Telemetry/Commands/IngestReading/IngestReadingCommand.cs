using MediatR;

namespace IoTTelemetryHub.Application.Telemetry.Commands.IngestReading;

public record IngestReadingCommand(Guid DeviceId, double Value, string Unit, DateTimeOffset RecordedAtUtc)
    : IRequest<Guid>;
