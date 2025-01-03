using System.ComponentModel.DataAnnotations;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Shared;
using Shared.DTOs.Order;
using Shared.Exceptions;

namespace Server.OrderController;

[ApiController]
[Authorize]
[Route("/api/order")]
public class OrderController(IOrderService orderService, ITrackingService trackingService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderTableResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<ActionResult<IEnumerable<OrderTableResponse>>> GetOrders(
        [FromQuery] List<string>? searchTerms)
    {
        var list = await orderService.GetOrders(searchTerms);
        return Ok(list);
    }

    [HttpPut("{orderId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> UpdateOrder(string orderId, [FromBody] OrderRequest orderRequest)
    {
        await orderService.UpdateOrder(orderId, orderRequest);
        return Ok();
    }

    [HttpGet("status")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderStatus>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<ActionResult<OrderStatus>> GetOrderStatuses()
    {
        var statuses = await orderService.GetAllOrderStatusesAsync();
        return Ok(statuses);
    }

    [HttpGet("{orderId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<ActionResult<OrderResponse>> GetOrderById(string orderId)
    {
        var order = await orderService.GetOrderById(orderId);
        return Ok(order);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> CreateOrder([FromBody] OrderRequest orderTable)
    {
        var createdOrder = await orderService.CreateOrder(orderTable);
        return CreatedAtAction(nameof(GetOrderById), new { orderId = createdOrder.Id }, createdOrder);
    }

    [HttpDelete("{orderId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> DeleteOrder(string orderId)
    {
        await orderService.DeleteOrder(orderId);
        return Ok("Order deleted successfully");
    }

    [HttpGet("{orderId}/tracking/{trackingNumber}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetOrderTrackingStatus(string orderId, string trackingNumber)
    {
        var order = await orderService.GetOrderById(orderId);
        if (order == null)
            throw new NotFoundException($"No order found with id {orderId}");
        if (string.IsNullOrWhiteSpace(order.Transporter))
            throw new ValidationException("Transporter is not set for this order.");

        var trackingStatus = await trackingService.GetTrackingStatusAsync(trackingNumber, order.Transporter);

        return Ok(trackingStatus);
    }
}