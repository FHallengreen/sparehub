using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Interfaces;
using Shared;
using Shared.Exceptions;
using Shared.Order;

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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderTableResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<ActionResult<IEnumerable<OrderTableResponse>>> GetOrders(
        [FromQuery] List<string>? searchTerms)
    {
        try
        {
            var orderService = GetDatabaseFactory().GetService<IOrderService>();
            var list = await orderService.GetOrders(searchTerms);
            return Ok(list);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (RepositoryException e)
        {
            return StatusCode(500, e.Message);
        }
    }


    [HttpPut("{orderId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> UpdateOrder(string orderId, [FromBody] OrderRequest orderRequest)
    {
        try
        {
            var orderService = GetDatabaseFactory().GetService<IOrderService>();
            await orderService.UpdateOrder(orderId, orderRequest);
            return Ok();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }

        catch (RepositoryException e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("status")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderStatus>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
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

    [HttpGet("{orderId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderStatus>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<ActionResult<OrderResponse>> GetOrderById(string orderId)
    {
        try
        {
            var orderService = GetDatabaseFactory().GetService<IOrderService>();
            var order = await orderService.GetOrderById(orderId);
            return Ok(order);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (RepositoryException e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> CreateOrder([FromBody] OrderRequest orderTable)
    {
        try
        {
            var orderService = GetDatabaseFactory().GetService<IOrderService>();
            var createdOrder = await orderService.CreateOrder(orderTable);

            return CreatedAtAction(nameof(GetOrderById), new { orderId = createdOrder.Id }, createdOrder);
        }
        catch (RepositoryException e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{orderId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> DeleteOrder(string orderId)
    {
        try
        {
            var orderService = GetDatabaseFactory().GetService<IOrderService>();
            await orderService.DeleteOrder(orderId);
            return Ok("Order deleted successfully");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, "Internal server error");
        }
    }

}
