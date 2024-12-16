using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Interfaces;
using Shared.DTOs.Dispatch;
using Shared.Exceptions;

namespace Server.DispatchController;

[ApiController]
[Route("/api/dispatch")]
[Authorize]
public class DispatchController(IDispatchService dispatchService) : ControllerBase
{

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(DispatchResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<ActionResult<DispatchResponse>> CreateDispatch([FromBody] DispatchRequest dispatchRequest)
    {
        var dispatchResponse = await dispatchService.CreateDispatch(dispatchRequest);
        return CreatedAtAction(nameof(GetDispatchById), new { dispatchId = dispatchResponse.Id }, dispatchResponse);
    }

    [HttpGet("{dispatchId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DispatchResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<ActionResult<DispatchResponse>> GetDispatchById(string dispatchId)
    {
        var dispatch = await dispatchService.GetDispatchById(dispatchId);
        return Ok(dispatch);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DispatchResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<ActionResult<IEnumerable<DispatchResponse>>> GetDispatches()
    {
        var dispatches = await dispatchService.GetDispatches();
        return Ok(dispatches);
    }

    [HttpPut("{dispatchId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> UpdateDispatch(string dispatchId, [FromBody] DispatchRequest dispatchRequest)
    {
        await dispatchService.UpdateDispatch(dispatchId, dispatchRequest);
        return NoContent();
    }

    [HttpDelete("{dispatchId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> DeleteDispatch(string dispatchId)
    {
        await dispatchService.DeleteDispatch(dispatchId);
        return NoContent();
    }
}