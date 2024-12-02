using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Shared;
using Shared.DTOs.Warehouse;

namespace Server.WarehouseController;

[ApiController]
[Route("api/warehouse")]
public class WarehouseController(IWarehouseService warehouseService) : ControllerBase
{
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WarehouseResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetWarehousesBySearch(string? searchQuery = null)
    {
        if (!string.IsNullOrEmpty(searchQuery))
        {
            var warehouses = await warehouseService.GetWarehousesBySearchQuery(searchQuery);
            return Ok(warehouses);
        }
        var allWarehouses = await warehouseService.GetWarehouses();
        return Ok(allWarehouses);
    }

    [HttpGet("{wareHouseId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WarehouseResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetWarehouseById(string wareHouseId)
    {
        var warehouse = await warehouseService.GetWarehouseById(wareHouseId);
        return Ok(warehouse);
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(WarehouseResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> CreateWarehouse([FromBody] WarehouseRequest warehouseRequest) {
        
        var createdWarehouse = await warehouseService.CreateWarehouse(warehouseRequest);
        return CreatedAtAction(nameof(GetWarehouseById), new { warehouseId = createdWarehouse.Id }, createdWarehouse);
    }
    
    [HttpPut("{warehouseId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WarehouseResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> UpdateWarehouse(string warehouseId, [FromBody] WarehouseRequest warehouseRequest)
    {
        var updatedWarehouse = await warehouseService.UpdateWarehouse(warehouseId, warehouseRequest);
        return Ok(updatedWarehouse);
    }
    
    [HttpDelete("{warehouseId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> DeleteWarehouse(string warehouseId)
    {
        await warehouseService.DeleteWarehouse(warehouseId);
        return Ok("Warehouse deleted successfully");
    }
}
