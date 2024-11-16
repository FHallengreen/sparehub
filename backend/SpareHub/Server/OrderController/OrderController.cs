using Domain;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Order;
using Shared;

namespace Server.OrderController;

[ApiController]
[Route("/api/order")]
public class OrderController(IServiceProvider serviceProvider) : ControllerBase
{
    private IDatabaseFactory GetDatabaseFactory()
    {
        return serviceProvider.GetRequiredService<IDatabaseFactory>();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderTableResponse>>> GetOrders(
        [FromQuery] List<string>? searchTerms)
    {
        try
        {
            var orderService = GetDatabaseFactory().GetService<IOrderService>();
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
            var orderService = GetDatabaseFactory().GetService<IOrderService>();
            await orderService.UpdateOrder(orderId, orderRequest);
            return NoContent();
        } catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("status")]
    public async Task<ActionResult<OrderStatus>> GetOrderStatuses()
    {
        try
        {
            var orderService = GetDatabaseFactory().GetService<IOrderService>();
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
            var orderService = GetDatabaseFactory().GetService<IOrderService>();
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
    public async Task<IActionResult> CreateOrder([FromBody] OrderRequest orderTable)
    {
        try
        {
            var orderService = GetDatabaseFactory().GetService<IOrderService>();
            var createdOrder = await orderService.CreateOrder(orderTable);

            return CreatedAtAction(nameof(GetOrderById), new { orderId = createdOrder.Id }, createdOrder);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{orderId:int}")]
    public ActionResult DeleteOrder(int orderId)
    {
        try
        {
            var orderService = GetDatabaseFactory().GetService<IOrderService>();
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
