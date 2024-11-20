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
    public async Task<ActionResult<DispatchResponse>> CreateDispatch([FromBody] DispatchRequest dispatchRequest, [FromQuery] string orderId)
    {
        try
        {
            var dispatchService = GetDatabaseFactory().GetService<IDispatchService>();
            var dispatchResponse = await dispatchService.CreateDispatch(dispatchRequest, orderId);
            return CreatedAtAction(nameof(GetDispatchById), new { dispatchId = dispatchResponse.Id }, dispatchResponse);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (RepositoryException e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet("{dispatchId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DispatchResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<ActionResult<DispatchResponse>> GetDispatchById(string dispatchId)
    {
        try
        {
            var dispatchService = GetDatabaseFactory().GetService<IDispatchService>();
            var dispatch = await dispatchService.GetDispatchById(dispatchId);
            return Ok(dispatch);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (RepositoryException e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DispatchResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<ActionResult<IEnumerable<DispatchResponse>>> GetDispatches()
    {
        try
        {
            var dispatchService = GetDatabaseFactory().GetService<IDispatchService>();
            var dispatches = await dispatchService.GetDispatches();
            return Ok(dispatches);
        }
        catch (RepositoryException e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPut("{dispatchId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> UpdateDispatch(string dispatchId, [FromBody] DispatchRequest dispatchRequest)
    {
        try
        {
            var dispatchService = GetDatabaseFactory().GetService<IDispatchService>();
            await dispatchService.UpdateDispatch(dispatchId, dispatchRequest);
            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (RepositoryException e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpDelete("{dispatchId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> DeleteDispatch(string dispatchId)
    {
        try
        {
            var dispatchService = GetDatabaseFactory().GetService<IDispatchService>();
            await dispatchService.DeleteDispatch(dispatchId);
            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (RepositoryException e)
        {
            return StatusCode(500, e.Message);
        }
    }
}