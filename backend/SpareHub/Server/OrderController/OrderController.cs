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
    public async Task<ActionResult<IEnumerable<OrderTableResponse>>> GetOrders([FromQuery] List<string>? searchTerms)
    {
        try
        {
            var list = await orderService.GetOrders(searchTerms);
            return Ok(list);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{orderId:int}")]
    public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] OrderRequest orderRequest)
    {
        try
        {
            await orderService.UpdateOrder(orderId, orderRequest);
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
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
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{orderId:int}")]
    public async Task<ActionResult<OrderResponse>> GetOrderById(int orderId)
    {
        try
        {
            var order = await orderService.GetOrderById(orderId);
            return Ok(order);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Order with ID '{orderId}' not found");
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderRequest orderTable)
    {
        try
        {
            await orderService.CreateOrder(orderTable);
            return Ok(orderTable);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{orderId:int}")]
    public IActionResult DeleteOrder(int orderId)
    {
        try
        {
            orderService.DeleteOrder(orderId);
            return Ok("Order deleted successfully");
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            return NotFound("Order not found");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Internal server error");
        }
    }

}
