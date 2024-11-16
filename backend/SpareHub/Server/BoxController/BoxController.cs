using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Interfaces;
using Shared;
using Shared.Order;

namespace Server.BoxController;

[ApiController]
[Route("api/order/{orderId}/box")]
public class BoxController(IBoxService boxService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateBox(string orderId, [FromBody] BoxRequest boxRequest)
    {
        try
        {
            var newBox = await boxService.CreateBox(boxRequest, orderId);
            return Ok(newBox);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetBoxesForOrder(string orderId)
    {
        var boxes = await boxService.GetBoxes(orderId);
        return Ok(boxes);
    }

    [HttpDelete("{boxIndex}")]
    public async Task<IActionResult> DeleteBox(string orderId, string boxIndex)
    {
        try
        {
            await boxService.DeleteBox(orderId, boxIndex);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
