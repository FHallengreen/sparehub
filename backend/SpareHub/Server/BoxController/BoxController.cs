using Microsoft.AspNetCore.Mvc;
using Service;
using Shared;

namespace Server.BoxController;

[ApiController]
[Route("api/orders/{orderId:int}/boxes")]
public class BoxController(IBoxService boxService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateBox(int orderId, [FromBody] BoxRequest boxRequest)
    {
        try
        {
            await boxService.CreateBox(boxRequest, orderId);
            return Ok();
        } catch (Exception e)
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
    
}
