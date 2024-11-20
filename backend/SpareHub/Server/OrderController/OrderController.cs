using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Interfaces;
using Shared;
using Shared.DTOs.Order;
using Shared.Exceptions;
using Shared.Order;

namespace Server.OrderController;

[ApiController]
[Route("/api/order")]
public class OrderController(IDatabaseFactory databaseFactory) : ControllerBase
{
    private readonly IOrderService _orderService = databaseFactory.GetService<IOrderService>();

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderTableResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<ActionResult<IEnumerable<OrderTableResponse>>> GetOrders(
        [FromQuery] List<string>? searchTerms)
    {
        var list = await _orderService.GetOrders(searchTerms);
        return Ok(list);
    }


    [HttpPut("{orderId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> UpdateOrder(string orderId, [FromBody] OrderRequest orderRequest)
    {
        await _orderService.UpdateOrder(orderId, orderRequest);
        return Ok();
    }

    [HttpGet("status")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderStatus>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<ActionResult<OrderStatus>> GetOrderStatuses()
    {
        var statuses = await _orderService.GetAllOrderStatusesAsync();
        return Ok(statuses);
    }

    [HttpGet("{orderId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderStatus>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<ActionResult<OrderResponse>> GetOrderById(string orderId)
    {
        var order = await _orderService.GetOrderById(orderId);
        return Ok(order);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> CreateOrder([FromBody] OrderRequest orderTable)
    {
        var createdOrder = await _orderService.CreateOrder(orderTable);
        return CreatedAtAction(nameof(GetOrderById), new { orderId = createdOrder.Id }, createdOrder);
    }

    [HttpDelete("{orderId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> DeleteOrder(string orderId)
    {
        await _orderService.DeleteOrder(orderId);
        return Ok("Order deleted successfully");
    }
}