using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Shared.Exceptions;
using Shared.Order;
using ValidationException = Shared.Exceptions.ValidationException;

namespace Server.BoxController;

[ApiController]
[Route("api/order/{orderId}/box")]
public class BoxController(IBoxService boxService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BoxResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> CreateBox(string orderId, [FromBody] BoxRequest boxRequest)
    {
        try
        {
            var newBox = await boxService.CreateBox(boxRequest, orderId);
            return CreatedAtAction(nameof(GetBoxesForOrder), new { orderId }, newBox);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<BoxResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetBoxesForOrder(string orderId)
    {
        try
        {
            var boxes = await boxService.GetBoxes(orderId);
            return Ok(boxes);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{boxId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> DeleteBox(string orderId, string boxId)
    {
        try
        {
            await boxService.DeleteBox(orderId, boxId);
            return Ok("Box deleted successfully");
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
