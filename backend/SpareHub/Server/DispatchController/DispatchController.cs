using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Interfaces;
using Shared.DTOs.Dispatch;
using Shared.Exceptions;

namespace Server.DispatchController;

[ApiController]
[Route("/api/dispatch")]
public class DispatchController(IServiceProvider serviceProvider) : ControllerBase
{
    private IDatabaseFactory GetDatabaseFactory()
    {
        return serviceProvider.GetRequiredService<IDatabaseFactory>();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(DispatchResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<ActionResult<DispatchResponse>> CreateDispatch([FromBody] DispatchRequest dispatchRequest,
        [FromQuery] string orderId)
    {
        var dispatchService = GetDatabaseFactory().GetService<IDispatchService>();
        var dispatchResponse = await dispatchService.CreateDispatch(dispatchRequest, orderId);
        return CreatedAtAction(nameof(GetDispatchById), new { dispatchId = dispatchResponse.Id }, dispatchResponse);
    }

    [HttpGet("{dispatchId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DispatchResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<ActionResult<DispatchResponse>> GetDispatchById(string dispatchId)
    {
        var dispatchService = GetDatabaseFactory().GetService<IDispatchService>();
        var dispatch = await dispatchService.GetDispatchById(dispatchId);
        return Ok(dispatch);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DispatchResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<ActionResult<IEnumerable<DispatchResponse>>> GetDispatches()
    {
        var dispatchService = GetDatabaseFactory().GetService<IDispatchService>();
        var dispatches = await dispatchService.GetDispatches();
        return Ok(dispatches);
    }

    [HttpPut("{dispatchId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> UpdateDispatch(string dispatchId, [FromBody] DispatchRequest dispatchRequest)
    {
        var dispatchService = GetDatabaseFactory().GetService<IDispatchService>();
        await dispatchService.UpdateDispatch(dispatchId, dispatchRequest);
        return NoContent();
    }

    [HttpDelete("{dispatchId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> DeleteDispatch(string dispatchId)
    {
        var dispatchService = GetDatabaseFactory().GetService<IDispatchService>();
        await dispatchService.DeleteDispatch(dispatchId);
        return NoContent();
    }
}