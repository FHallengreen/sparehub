using Microsoft.AspNetCore.Mvc;
using Service;
using Shared;

[ApiController]
[Route("api/order/{orderId:int}/box")]
public class BoxController(IBoxService boxService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateBox(int orderId, [FromBody] BoxRequest boxRequest)
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
    public async Task<IActionResult> GetBoxesForOrder(int orderId)
    {
        var boxes = await boxService.GetBoxes(orderId);
        return Ok(boxes);
    }

    [HttpDelete("{boxIndex}")]
    public async Task<IActionResult> DeleteBox(int orderId, int boxIndex)
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