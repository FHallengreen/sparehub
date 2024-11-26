using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Interfaces;
using Shared;

namespace Server.WarehouseController;

[ApiController]
[Route("api/warehouse")]
public class WarehouseController(IDatabaseFactory databaseFactory) : ControllerBase
{
    private readonly IWarehouseService _warehouseService = databaseFactory.GetService<IWarehouseService>();
    
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WarehouseResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetWarehouses(string? searchQuery = null)
    {
        var warehouses = await _warehouseService.GetWarehousesBySearchQuery(searchQuery);
        return Ok(warehouses);
    }

    [HttpGet("{wareHouseId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WarehouseResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetWarehouseById(string wareHouseId)
    {
        var warehouse = await _warehouseService.GetWarehouseById(wareHouseId);
        return Ok(warehouse);
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(WarehouseResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> CreateWarehouse([FromBody] WarehouseRequest warehouseRequest) {
        
        var createdWarehouse = await _warehouseService.CreateWarehouse(warehouseRequest);
        return CreatedAtAction(nameof(GetWarehouseById), new { warehouseId = createdWarehouse.Id }, createdWarehouse);
    }
    
    [HttpPut("{warehouseId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WarehouseResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> UpdateWarehouse(string warehouseId, [FromBody] WarehouseRequest warehouseRequest)
    {
        var updatedWarehouse = await _warehouseService.UpdateWarehouse(warehouseId, warehouseRequest);
        return Ok(updatedWarehouse);
    }
    
    [HttpDelete("{warehouseId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> DeleteWarehouse(string warehouseId)
    {
        await _warehouseService.DeleteWarehouse(warehouseId);
        return Ok("Warehouse deleted successfully");
    }
}
