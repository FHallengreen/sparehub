using Domain;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Service;
using Shared;

namespace Server.OrderController;

[ApiController]
[Route("/api/orders")]
public class OrderController(IOrderService orderService) : ControllerBase
{
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        try
        {
            var list = await orderService.GetOrders();
            return Ok(list);
        } catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Order>> PostOrder([FromBody] OrderRequest order)
    {
        try
        {
            await orderService.CreateOrder(order);
            return Ok(order);
        } catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Internal server error");
        }

    }
}
