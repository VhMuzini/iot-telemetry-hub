using FluentValidation;

namespace IoTTelemetryHub.Application.Telemetry.Commands.IngestReading;

public class IngestReadingCommandValidator : AbstractValidator<IngestReadingCommand>
{
    public IngestReadingCommandValidator()
    {
        RuleFor(x => x.DeviceId).NotEmpty();
        RuleFor(x => x.Unit).NotEmpty().MaximumLength(16);
        RuleFor(x => x.RecordedAtUtc)
            .LessThanOrEqualTo(_ => DateTimeOffset.UtcNow.AddMinutes(1))
            .WithMessage("Reading timestamp cannot be in the future.");
    }
}
