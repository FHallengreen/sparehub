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
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrders([FromQuery] List<string>? searchTerms) 
    {
        try
        {
            var list = await orderService.GetOrders(searchTerms); 
            return Ok(list);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpGet("statuses")]
    public async Task<ActionResult<OrderStatus>> GetOrderStatuses()
    {
        try
        {
            var statuses = await orderService.GetAllOrderStatusesAsync();
            return Ok(statuses);
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
