using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Interfaces;
using Shared.DTOs.Order;
using Shared.Exceptions;

namespace Server.BoxController;

[ApiController]
[Route("api/order/{orderId}/box")]
public class BoxController(IBoxService boxService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BoxResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> CreateBox(string orderId, [FromBody] BoxRequest boxRequest)
    {
        var newBox = await boxService.CreateBox(boxRequest, orderId);
        return CreatedAtAction(nameof(GetBoxesForOrder), new { orderId }, newBox);
    }

    [HttpPut("{boxId}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BoxResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateBox(string orderId, List<BoxRequest> boxRequests)
    {
        await boxService.UpdateBoxes(orderId, boxRequests);
        return Ok("Box updated successfully");
    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<BoxResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBoxesForOrder(string orderId)
    {
        var boxes = await boxService.GetBoxes(orderId);
        return Ok(boxes);
    }

    [HttpDelete("{boxId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> DeleteBox(string orderId, string boxId)
    {
        await boxService.DeleteBox(orderId, boxId);
        return Ok("Box deleted successfully");
    }
}