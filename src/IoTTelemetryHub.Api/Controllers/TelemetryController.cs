using IoTTelemetryHub.Application.Telemetry.Commands.IngestReading;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IoTTelemetryHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TelemetryController : ControllerBase
{
    private readonly ISender _sender;

    public TelemetryController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>Ingests a single sensor reading from a device.</summary>
    [HttpPost]
    public async Task<IActionResult> Ingest([FromBody] IngestReadingCommand command, CancellationToken cancellationToken)
    {
        var readingId = await _sender.Send(command, cancellationToken);
        return Accepted(new { id = readingId });
    }
}
