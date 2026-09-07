using IoTTelemetryHub.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IoTTelemetryHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{
    private readonly IApplicationDbContext _db;

    public DevicesController(IApplicationDbContext db)
    {
        _db = db;
    }

    /// <summary>Lists registered devices and their current status.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var devices = await _db.Devices
            .Select(d => new
            {
                d.Id,
                d.Name,
                d.DeviceType,
                d.Location,
                d.Status,
                d.LastSeenAtUtc
            })
            .ToListAsync(cancellationToken);

        return Ok(devices);
    }
}
