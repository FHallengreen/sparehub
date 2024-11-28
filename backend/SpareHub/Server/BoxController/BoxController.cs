using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Interfaces;
using Shared.DTOs.Order;
using Shared.Exceptions;
using Shared.Order;

namespace Server.BoxController;

[ApiController]
[Route("api/order/{orderId}/box")]
public class BoxController(IDatabaseFactory databaseFactory) : ControllerBase
{

    private readonly IBoxService _boxService = databaseFactory.GetService<IBoxService>();


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BoxResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> CreateBox(string orderId, [FromBody] BoxRequest boxRequest)
    {
        var newBox = await _boxService.CreateBox(boxRequest, orderId);
        return CreatedAtAction(nameof(GetBoxesForOrder), new { orderId }, newBox);
    }

    [HttpPut("{boxId}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BoxResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateBox(string orderId, List<BoxRequest> boxRequests)
    {
        await _boxService.UpdateBoxes(orderId,boxRequests);
        return Ok("Box updated successfully");
    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<BoxResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBoxesForOrder(string orderId)
    {
        var boxes = await _boxService.GetBoxes(orderId);
        return Ok(boxes);
    }

    [HttpDelete("{boxId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> DeleteBox(string orderId, string boxId)
    {
        await _boxService.DeleteBox(orderId, boxId);
        return Ok("Box deleted successfully");
    }
}