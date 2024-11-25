using Microsoft.AspNetCore.Mvc;
using Service.Warehouse;
using Shared;
using Shared.Exceptions;

namespace Server.WarehouseController;

[ApiController]
[Route("api/warehouse")]
public class WarehouseController(IWarehouseService warehouseService) : ControllerBase
{
    
    
    [HttpGet("search")]
    public async Task<IActionResult> GetWarehouses(string? searchQuery = null)
    {
        try
        {
            var warehouses = await warehouseService.GetWarehousesBySearchQuery(searchQuery);

            if (!warehouses.Any())
            {
                return NotFound("No warehouses found matching the search criteria.");
            }
            return Ok(warehouses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{warehouseId}")]
    public async Task<IActionResult> GetWarehouseById(string wareHouseId)
    {
        try
        {
            var warehouse = await warehouseService.GetWarehouseById(wareHouseId);
            return Ok(warehouse);
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
    public async Task<IActionResult> CreateWarehouse([FromBody] WarehouseRequest warehouseRequest) {
        
        try
        {
            var createdWarehouse = await warehouseService.CreateWarehouse(warehouseRequest);

            return CreatedAtAction(nameof(GetWarehouseById), new { warehouseId = createdWarehouse.Id }, createdWarehouse);
        }
        catch (RepositoryException e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
}
