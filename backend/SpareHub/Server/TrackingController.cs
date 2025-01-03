using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Server;

[ApiController]
[Route("api/tracking")]
public class TrackingController (ITrackingService trackingService) : ControllerBase
{
    [HttpGet("{trackingNumber}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetOrderTrackingStatus(string trackingNumber)
    {

        var trackingStatus = await trackingService.GetTrackingStatusAsync(trackingNumber);

        return Ok(trackingStatus);
    }
}